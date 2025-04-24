using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class Teacher
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public string? designation { get; set;}
        public string? expertise { get; set;}
        public string? registrationNo {  get; set; }
        public bool? status { get; set; }
        public string? gender {get; set;}
        public string? UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
