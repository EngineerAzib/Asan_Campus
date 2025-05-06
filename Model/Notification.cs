using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Notification
    {
        public int Id { get; set; } // 

        public bool IsRead { get; set; }

        public string Message { get; set; }

       public  string date { get;set; }

        public DateTime Timestamp { get; set; }
        public bool? IsSent { get; set; }
        public string Title { get; set; }

        public string Type { get; set; }
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public int SemesterID { get; set; }
        [ForeignKey("SemesterID")]
        public Semester Semester { get; set; }
    }

}
