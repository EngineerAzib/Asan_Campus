using System.ComponentModel.DataAnnotations;

namespace Asan_Campus.Model
{
    public class Prerequisite
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to the Course that requires prerequisites
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Foreign key to the prerequisite course
        public int PrerequisiteCourseId { get; set; }
        public Course PrerequisiteCourse { get; set; }
    }
}
