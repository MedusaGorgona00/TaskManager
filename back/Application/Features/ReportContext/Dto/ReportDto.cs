namespace Application.Features.ReportContext.Dto
{
    public class ReportDto
    {
        public int JobId { get; set; }
        public int NotificationId { get; set; }
        public DateTime? JobCreatedDate { get; set; }
        public DateTime? JobEndDate { get; set; }
        public string Text { get; set; }
        public DateTime? NotificationCreatedDate { get; set; }
        public int JobTypeId { get; set; }
        public string Description { get; set; }
        public string? Message { get; set; }
    }
}
