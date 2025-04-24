using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public string Day { get; set; }
        public string TimeSlot { get; set; }
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public string Section { get; set; }
    }
}
