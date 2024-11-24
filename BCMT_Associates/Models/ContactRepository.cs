namespace BCMT_Associates.Models
{
    public class ContactRepository
    {
        // Static field to store contact information
        public static Contact ContactInfo { get; set; } = new Contact
        {
            Location = "Chakdara",
            Email = "admin@example.com",
            ContactNumber = "03494566945"
        };
    }
}
