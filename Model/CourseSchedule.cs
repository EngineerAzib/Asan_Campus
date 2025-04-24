using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class CourseSchedule
    {
        public int Id { get; set; }
        public string days { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string room { get; set; }
        public int courseId { get; set; }
        [ForeignKey("courseId")]
        public Course course { get; set; }

    }
}
