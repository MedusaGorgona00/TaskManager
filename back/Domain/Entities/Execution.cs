namespace Domain.Entities
{
    public class Execution
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public ExecutionHistory? ExecutionHistory { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
