namespace Core.Entities;

public class ServiceRecordProcedureStep : BaseEntity
{
    public int ServiceProcedureId { get; set; }
    public ServiceRecordProcedures? ServiceRecordProcedures { get; set; }

    public int ProcedureId { get; set; }
    public Procedure? Procedure { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
