using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }

        [Required, StringLength(50)]
        public string TimeSlot { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Required, StringLength(100)]
        public string Venue { get; set; }

        // Foreign Key
        public int ExamScheduleId { get; set; }
        public ExamSchedule ExamSchedule { get; set; }
    }

}
