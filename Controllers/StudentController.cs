using Asan_Campus.Model;
using Asan_Campus.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ImplementationSevices _implementationSevices;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public StudentController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IPasswordHasher<IdentityUser> passwordHasher, ImplementationSevices implementationSevices)
        {
            _implementationSevices = implementationSevices;
            _context = context;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudent request)
        {
            var Id = (_context.Students.Count() + 1);
            var user = new IdentityUser { UserName = Id + "-" + request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            var roleResult = await _userManager.AddToRoleAsync(user, "STUDENT");
            //var department = _context.Departments.FirstOrDefault(x => x.Id == request.DepartmentId);
            //var rollNo = string.Join(".", department.DepartmentName
            //.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            //   .Select(namePart => char.ToUpper(namePart[0]))) + ".";
            Random randomNo = new Random();
            int ranNo = randomNo.Next(1001, 9999);
            var endrollNo = "NED/" + Id + ranNo + "/" + DateTime.Now.Year;

            if (result.Succeeded)
            {
                var userId = user.Id;

                var student = new Student
                {
                    UserId = userId,
                    Name = request.Name,
                    Phone = request.Phone,
                    Father_Name = request.Father_Name,
                    CNIC = request.CNIC,
                    DepartmentId = request.DepartmentId,
                    Gender = request.Gender,
                    DOB = request.DOB,
                    Address = request.DOB,
                    EndrollementNo = endrollNo,
                    RollNo = request.RollNo,

                    _isActive = true,
                    _isDelete = false

                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Registration successful", StudentId = student.Id });
            }
            return Ok(new { Errors = result.Errors });
        }
        [HttpPost]
        public IActionResult VerifyPassword(int stdid, string inputPassword)
        {
            var student = _context.Students
                .Where(x => x.Id == stdid)
                .Select(x => new
                {
                    x.User.Email,
                    x.User.PasswordHash
                })
                .FirstOrDefault();

            if (student == null)
            {
                return NotFound("Student not found");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(null, student.PasswordHash, inputPassword);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                return Ok("Password is correct");
            }
            else
            {
                return BadRequest("Incorrect password");
            }
        }
            [HttpGet("GetAllStudent")]
        public IActionResult GetAllStudent()
        {
            var res = _context.Students.Select(x => new
            {
                x.Id,
                x.Name,
                x.EndrollementNo,
                x.RollNo,
            }).ToList();
            return Ok(res);
        }
        [HttpGet("GetStudentById")]
        public IActionResult GetStudentById(int stdid)
        {
           var basicInfo= _implementationSevices.GetStudentBasicInfo(stdid);
           var academics= _implementationSevices.GetAcademicDetail(stdid);
          var attendance= _implementationSevices.CourseAttendance(stdid);

            return Ok(new
            {
                basicInfo,
                academics,
                attendance,
            });
        }
        [HttpGet("GetCurrentSemester")]
        public IActionResult GetCurrentSemester(int stdid)
        {
            var res = _context.Students.Where(w => w.Id == stdid).Select(x => new
            {
                semesterName="Semester "+x.Semester,
                id=x.Semester
            }).FirstOrDefault();
            return Ok(res);
        }
        [HttpGet("GetCurrentCourse")]
        public IActionResult GetCurrentCourse(int stdid,int semid)
        {
            var res = _context.AcadmicDetails.Where(w => w.studentId == stdid && w.semesterId== semid).Include(x => x.course).Select(x=>new
            {
                x.course.Name,
                x.course.Id,
            }).ToArray();
            return Ok(res);
        }


    }
}
