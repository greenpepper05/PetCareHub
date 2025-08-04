namespace Infrastructure.Data;

public static class ProcedureSeeder
{
    public static List<string> GetStepsForService(string serviceName)
    {
        return serviceName.ToLower() switch
        {
            "pet gromming" => new List<string> {
                "Brushing", "Bathing", "Drying", "Ear Cleaning", "Nail Trimming", "Coat Trimming"
            },
            _ => new List<string>()
        };
    }
}
