namespace BCMT_Associates.Models
{
    public class ResetPassword
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ExpiresOn { get; set; }
    }
}
