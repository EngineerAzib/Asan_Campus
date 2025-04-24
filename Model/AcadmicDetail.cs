using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class AcadmicDetail
    {
        public int id { get;set; }
        public double gpa { get; set; } 
        public string grade {  get; set; }

        public bool? complete { get; set; }
        public int studentId { get; set; }
        [ForeignKey("studentId")]
        public Student students { get; set; }   
        public int courseId { get; set; }

        [ForeignKey("courseId")]
        public Course course { get; set; }
        public int semesterId { get; set; }
        [ForeignKey("semesterId")]
        public Semester semester { get; set; }
    }
}
