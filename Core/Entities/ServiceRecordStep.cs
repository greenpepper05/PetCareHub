namespace Core.Entities;

public class ServiceRecordStep : BaseEntity
{
    public int ServiceRecordId { get; set; }
    public ServiceRecord? ServiceRecord { get; set; }

    public int ProcedureId { get; set; }
    public Procedure? Procedure { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
