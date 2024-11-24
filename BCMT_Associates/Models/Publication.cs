using System.ComponentModel.DataAnnotations;

namespace BCMT_Associates.Models
{

    public class Publication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string JournalName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; }

        [Required]
        [StringLength(20)]
        public string ISSN { get; set; }
        public bool IsVisibleOnMainPage { get; set; }  // New property

    }
}
