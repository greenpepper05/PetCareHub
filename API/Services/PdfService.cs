using Core.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace API.Services;

public class PdfService(IWebHostEnvironment env)
{
    public string GenerateServiceRecordPdf(ServiceRecord record, Pet pet)
    {
        var fileName = $"ServiceRecord_{record.Id}.pdf";
        var folderPath = Path.Combine(env.WebRootPath, "assets","reports");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        QuestPDF.Settings.License = LicenseType.Community;

        var pdfBytes = Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(ts => ts.FontSize(12));

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text("Service Completion Report").FontSize(22).Bold();

                    col.Item().LineHorizontal(1);

                    col.Item().Text($"Service Record ID: {record.Id}").Bold();
                    col.Item().Text($"Pet Name: {pet.Name}");
                    col.Item().Text($"Service: {record.Service?.Name}");
                    col.Item().Text($"Clinic: {record.Clinic?.ClinicName}");
                    col.Item().Text($"Completed on: {DateTime.UtcNow:MMMM dd, yyyy hh:mm tt}");
                    col.Item().Text($"Status: Completed");

                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    col.Item().Text("Notes:").Bold().FontSize(14);

                    col.Item().Text(record.Note ?? "No additional notes");
                });
            });
        }).GeneratePdf();

        File.WriteAllBytes(filePath, pdfBytes);

        return $"/assets/reports/{fileName}";
    }
}
