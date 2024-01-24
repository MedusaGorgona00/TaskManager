namespace Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public int JobTypeId { get; set; } //JobTypeRefID 
        public JobType? JobType { get; set; }
        public string? Description { get; set; }

        public ICollection<Execution> Executions { get; set; }
    }
}
