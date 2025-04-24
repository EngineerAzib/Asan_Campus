using System.ComponentModel.DataAnnotations;

namespace Asan_Campus.Model
{
    public class Year
    {
        [Key]
        public int YearId { get; set; }

        [Required]
        public int YearNumber { get; set; }  

        [Required, StringLength(50)]
        public string YearName { get; set; }
    }
}
