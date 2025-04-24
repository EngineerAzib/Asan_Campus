using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Feedback
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public string Section { get; set; }
        public Double Rating { get; set; }
        public Double TeachingRating { get; set; }
        public Double KnowledgeRating { get; set; }
        public Double CommunicationRating { get; set; }
    }
}
