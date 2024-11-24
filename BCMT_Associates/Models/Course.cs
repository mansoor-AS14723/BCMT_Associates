using System.ComponentModel.DataAnnotations;

namespace BCMT_Associates.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public byte[]? ImageData { get; set; }

        public bool IsVisibleOnMainPage { get; set; }  // New property

    }
}
