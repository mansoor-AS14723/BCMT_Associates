using System.ComponentModel.DataAnnotations.Schema;

namespace BCMT_Associates.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public char? Gender { get; set; }
        public string? Email { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? ContactNo { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; } 
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public bool? IsActive { get;set; }
        public bool? IsDeleted { get; set; }
       

    }

	public class Users
	{
		public int ID { get; set; }
		public int RoleId { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }
		public string? Gender { get; set; }
		public string? Email { get; set; }
		public Nullable<System.DateTime> DOB { get; set; }
		public string? MobileNo { get; set; }
		public string? PhoneNo { get; set; }
		public string? CNIC { get; set; }
		public string? CurrentAddress { get; set; }
		public string? PermanentAddress { get; set; }
		public Nullable<int> CityId { get; set; }
		public string? Password { get; set; }
		public string? ConfirmPassword { get; set; }
		public Nullable<System.DateTime> CreatedOn { get; set; }
		public Nullable<int> CreatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public Nullable<int> UpdatedBy { get; set; }
		[NotMapped]
		public string? Token { get; set; }
		public string? ProfilePicture { get; set; }

		public Roles? roles { get; set; }
		

	}
}
