namespace BCMT_Associates.Models
{
    public class ActivityLog
    {
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }

}
