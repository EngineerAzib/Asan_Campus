using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Internship
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }

        [MaxLength(255)]
        public string? Location { get; set; } // Nullable string

        [MaxLength(50)]
        public string? Duration { get; set; } // Nullable string

        [Required]
        public DateTime ApplicationDeadline { get; set; }

        public DateTime PostedDate { get; set; } // Nullable DateTime

        public int Stippend { get; set; }
        public int Position { get; set; }
        public int Year { get; set; }
        public int departmentId { get; set; }

        [ForeignKey("departmentId")] 
        public Department department { get; set; }

        public bool? IsActive { get; set; } // Nullable boolean

    }
}
