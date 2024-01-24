namespace Domain.Entities
{
    public class ExecutionHistory
    {
        public int Id { get; set; }
        public int ExecutionId { get; set; }
        public Execution? Execution { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
