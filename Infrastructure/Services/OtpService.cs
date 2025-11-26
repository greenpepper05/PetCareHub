using System.Collections.Concurrent;
using Infrastructure.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class OtpData
{
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiryTime { get; set; }
}

public class OtpService(IServiceScopeFactory scopeFactory) : IOtpService
{
    private static readonly ConcurrentDictionary<string, OtpData> otpStore = new();
    private const int OtpDurationMinutes = 5;
    private const int OtpLength = 6;
    public async Task GenerateAndSendOtpAsync(string email)
    {
        var random = new Random();
        string code = random.Next((int)Math.Pow(10, OtpLength - 1), (int)Math.Pow(10, OtpLength)).ToString();

        var otpData = new OtpData
        {
            Code = code,
            ExpiryTime = DateTime.UtcNow.AddMinutes(OtpDurationMinutes)
        };

        otpStore.AddOrUpdate(email, otpData, (key, existingVal) => otpData);

        string subject = "PetCareHub: Your Registration Verification Code";
        string body = $"Your verification code is: <b>{code}</b>, It is valid for {OtpDurationMinutes} minutes.";

        using var scope = scopeFactory.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        await emailService.SendEmailAsync(email, subject, body);
    }

    public Task InvalidateOtpAsync(string email)
    {
        otpStore.TryRemove(email, out _);
        return Task.CompletedTask;
    }

    public Task<bool> ValidateOtpAsync(string email, string otp)
    {
        if (!otpStore.TryGetValue(email, out var storedData))
        {
            return Task.FromResult(false);
        }

        if (storedData.ExpiryTime < DateTime.UtcNow)
        {
            otpStore.TryRemove(email, out _);
            return Task.FromResult(false);
        }

        if (storedData.Code == otp)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
