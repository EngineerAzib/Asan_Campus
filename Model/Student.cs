using Microsoft.AspNetCore.Identity;

namespace Asan_Campus.Model
{
    public class Student
    {
        public int Id { get; set; }

       
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        
        public string Name { get; set; }
        public string Father_Name { get; set; }
        public string CNIC { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int Semester { get; set; } = 1; // Default to Semester 1
        public string Address { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string EndrollementNo { get; set; }
        public string RollNo { get; set; }
        public bool _isActive { get; set; }
        public bool _isDelete { get; set; }
    }
}
