using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Credits { get; set; }
         
        public bool isElective { get; set; }

        public string InitialName { get; set; }
        // Foreign key for Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // Foreign key for Semester
        public int SemesterId { get; set; }
        public string? type { get;set; }
        public Semester Semester { get; set; }

        // Self-referencing many-to-many relationship for prerequisites
        public ICollection<Prerequisite> Prerequisites { get; set; }
        public ICollection<Prerequisite> RequiredFor { get; set; }
        public ICollection<DepartmentSemester> DepartmentSemesters { get; set; }
        public ICollection<CourseSchedule> CourseSchedules { get; set; } 

        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }
       
       

    }
}
