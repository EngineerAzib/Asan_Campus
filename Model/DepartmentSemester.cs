using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class DepartmentSemester
    {
        public int DepartmentId { get; set; }  // Foreign key to Department
        public Department Department { get; set; }  // Navigation property to Department

        public int SemesterId { get; set; }  // Foreign key to Semester
        public Semester Semester { get; set; }  // Navigation property to Semester
        public int CourseId { get; set; }  // Foreign key to Semester
        public Course Course { get; set; }  // Navigation property to Semester
    }

}
