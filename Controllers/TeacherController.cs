using Asan_Campus.Model;
using Asan_Campus.Service;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ImplementationSevices _implementationSevices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        public TeacherController(ApplicationDbContext context, ImplementationSevices implementationSevices, UserManager<IdentityUser> userManager, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _context = context;
            _implementationSevices = implementationSevices;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        [HttpGet("GetAllTeacher")]
        public IActionResult GetAllTeacher()
        {
            var res = _context.Teachers.Select(x => new
            {
                x.Id,
                x.designation,
                department=x.Department.DepartmentName
            }).ToList();
            return Ok(new
            {
                Teacher = res
            }) ;
        }
        [HttpGet("GetTeacher")]
        public IActionResult GetTeacher(int teacherId)
        {
            var teacher = _implementationSevices.GetTeacher(teacherId);
            var schedule = _implementationSevices.GetSchedule(teacherId);
            var coursesAttendance=_implementationSevices.GetcourseAttendanse(teacherId);
            var feedbacks=_implementationSevices.GetFeedback(teacherId);
            return Ok(new
            {
                TeacherData= teacher,
                schedule,
                coursesAttendance, feedbacks
            });

        }
        [HttpPost("AddTeacher")]
        public async Task<IActionResult> AddTeacher(AddTeacher request)
        {
            // Create Identity User
            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Errors = result.Errors });
            }

            // Assign Teacher Role
            var roleResult = await _userManager.AddToRoleAsync(user, "TEACHER");

            // Generate Unique Registration Number
            Random randomNo = new Random();
            int ranNo = randomNo.Next(1001, 9999);
            var registrationNo = $"NED/{ranNo}/{DateTime.Now.Year}";

            // Create Teacher Object
            var teacher = new Teacher
            {
                UserId = user.Id,
                Name = request.Name,
                designation = request.designation,
                expertise = request.expertise,
                gender = request.gender,
                DepartmentID = request.DepartmentID,
                registrationNo = registrationNo,
                status = true
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return Ok(new { Result = "Registration successful", TeacherId = teacher.Id });
        }
        [HttpPost("AddTeacherSchedule")]
        public  IActionResult AddTeacherSchedule(List<AddTeacherSchedule> teacherSchedule)
        {
            foreach (var item in teacherSchedule)
            {
                var schedule = new Schedule()
                {
                    DepartmentID = item.DepartmentID,
                    TeacherId = item.TeacherId,
                    Day = item.Day,
                    Section = item.Section,
                    TimeSlot = item.TimeSlot
                };
                _context.Schedules.Add(schedule);
            }
            _context.SaveChanges();
            return Ok("Added Successfully");
        }
        [HttpPost("AddTeacherAttendance")]
        public IActionResult AddTeacherAttendance(List<AddTeacherAttendance> teacherAttendance)
        {
            foreach (var item in teacherAttendance)
            {
                var attendance = new Model.CourseAttendance()
                {
                    DepartmentID = item.DepartmentID,
                    TeacherId = item.TeacherId,
                    ClassesTaken=item.ClassesTaken,
                    Section=item.Section,
                    CourseId=item.CourseId,
                    TotalClasses=item.TotalClasses,
                   
                };
                _context.CourseAttendances.Add(attendance);
            }
            _context.SaveChanges();
            return Ok("Added Successfully");
        }

    }
}
