using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class SemesterRegistration
    {
        public int id {  get; set; }    
        public int? studentRegistrated {  get; set; }    
        public int MaxStudent {  get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }   
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public int SemesterID { get; set; }
        [ForeignKey("SemesterID")]
        public Semester Semester { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
       


    }
}
