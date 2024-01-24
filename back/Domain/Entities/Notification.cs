namespace Domain.Entities
{
    public  class Notification
    {
        public int Id { get; set; }
        public int ExecutionId { get; set; }   //SourceObjectID
        public Execution? Execution { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
