using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asan_Campus.Model
{
    public class StudentAttendance
    {

        [Key]
        public int StudentAttendanceID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int SemesterID { get; set; }

        [Required]
        public int TotalClasses { get; set; }

        [Required]
        public int AttendedClasses { get; set; }

        // Navigation Properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("SemesterID")]
        public virtual Semester Semester { get; set; }
    }
}
