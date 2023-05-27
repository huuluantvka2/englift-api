using EngLift.Model.Interfaces;

namespace EngLift.Model.Abstracts
{
    public abstract class AuditBase : IAudit
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
