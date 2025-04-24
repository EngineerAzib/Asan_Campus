using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class ExamSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required, StringLength(20)]
        public string ExamDay { get; set; }

        // Foreign Key
        public int YearId { get; set; }
        public Year Year { get; set; }

        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public int SemesterID { get; set; }
        [ForeignKey("SemesterID")]

        public Semester Semester { get; set; }

        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
