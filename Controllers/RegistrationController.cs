using Asan_Campus.Model;
using Asan_Campus.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ImplementationSevices _implementationSevices;

        public RegistrationController(ApplicationDbContext context, ImplementationSevices implementationSevices)
        {
            _context = context;
            _implementationSevices = implementationSevices;
        }
        [HttpPost("CreateAddRegistration")]

        public IActionResult CreateAddRegistration(CreateRegistration createRegistration)
        {
            var add = new SemesterRegistration()
            {
                  CourseId=createRegistration.CourseId,
                  DepartmentID=createRegistration.DepartmentID,
                  SemesterID=createRegistration.SemesterID,
                  startDate=createRegistration.startDate,
                  endDate=createRegistration.endDate,
                  MaxStudent=createRegistration.MaxStudent
            };
            _context.SemesterRegistration.Add(add);
            _context.SaveChanges();
            return Ok(new { success = true, message = "Successfully Added" });
        }
        [HttpGet("GetSemesterRegistration")]
        public IActionResult GetSemesterRegistration(string deptCode, int semId)
        {
            var res = _context.SemesterRegistration.Where(x => x.Department.DepartmentCode == deptCode && x.SemesterID == semId).Select(x => new
            {
                registrationDeadline=x.endDate,
                startDate=x.startDate,
                courses = new
                {
                    code=x.Course.InitialName,
                    name=x.Course.Name,
                    creditHours=x.Course.Credits,
                    instructor=x.Course.Teacher.Name,
                    maxStudents=x.MaxStudent,
                    prerequisites=x.Course.Prerequisites.Select(x=>x.Course.Name).ToList(),
                }
            }).ToList();
            return Ok(res);
        }

        [HttpGet("StudentRegistration")]
        public IActionResult StudentRegistration()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");

            var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
            if (student == null)
                return NotFound("Student not found");

            // All course IDs that were attempted by the student
            var allAttemptedCourseIds = _context.AcadmicDetails
                .Where(x => x.studentId == student.Id)
                .Select(x => x.courseId)
                .ToHashSet();

            // All courses attempted with GPA 0 (these must be excluded no matter what)
            var gpaZeroCourseIds = _context.AcadmicDetails
                .Where(x => x.studentId == student.Id && x.gpa == 0)
                .Select(x => x.courseId)
                .ToHashSet();

            // Courses that should show for registration:
            // - Either never attempted
            // - OR previously attempted with GPA > 0 and <= 3
            var validRetakeCourseIds = _context.AcadmicDetails
                .Where(x => x.studentId == student.Id && x.gpa <= 3 && x.gpa > 0)
                .Select(x => x.courseId)
                .ToHashSet();

            var eligibleCourses = _context.SemesterRegistration
                .Where(x =>
                    x.DepartmentID == student.DepartmentId &&
                    x.SemesterID == student.Semester &&
                    (
                        !allAttemptedCourseIds.Contains(x.Course.Id) ||           // Never registered
                        validRetakeCourseIds.Contains(x.Course.Id)               // Retake allowed (GPA ≤ 3 and > 0)
                    ) &&
                    !gpaZeroCourseIds.Contains(x.Course.Id)                      // Exclude all with GPA = 0
                )
                .Include(x => x.Department)
                .Include(x => x.Semester)
                .Include(x => x.Course).ThenInclude(c => c.Teacher)
                .Include(x => x.Course).ThenInclude(c => c.Prerequisites).ThenInclude(p => p.PrerequisiteCourse)
                .Include(x => x.Course).ThenInclude(c => c.CourseSchedules)
                .ToList();

            var data = eligibleCourses
                .GroupBy(x => x.Department.DepartmentCode)
                .ToDictionary(deptGroup => deptGroup.Key, deptGroup => new
                {
                    name = deptGroup.First().Department.DepartmentName,
                    semesters = deptGroup
                        .GroupBy(x => x.Semester.Id)
                        .ToDictionary(semGroup => semGroup.Key, semGroup => new
                        {
                            semester = semGroup.Any() ? $"{semGroup.First().Semester.Id}th Semester" : "Unknown Semester",
                            registrationDeadline = semGroup.FirstOrDefault()?.endDate,
                            startDate = semGroup.FirstOrDefault()?.startDate,

                            courses = semGroup.Select(c =>
                            {
                                var scheduleEntry = c.Course.CourseSchedules.FirstOrDefault();
                                return new
                                {
                                    code = c.Course.InitialName,
                                    name = c.Course.Name,
                                    creditHours = c.Course.Credits,
                                    type = c.Course.type,
                                    instructor = c.Course.Teacher?.Name ?? "Teacher",
                                    maxStudents = c.MaxStudent,
                                    prerequisites = c.Course.Prerequisites?.Select(p => p.PrerequisiteCourse.InitialName).ToList() ?? new List<string>(),
                                    schedule = new
                                    {
                                        days = !string.IsNullOrEmpty(scheduleEntry?.days) ? scheduleEntry.days.Split(',') : new string[] { },
                                        time = (!string.IsNullOrEmpty(scheduleEntry?.startTime) && !string.IsNullOrEmpty(scheduleEntry?.endTime))
                                            ? $"{scheduleEntry.startTime} - {scheduleEntry.endTime}"
                                            : "Time not available",
                                        room = scheduleEntry?.room ?? "Room not assigned"
                                    }
                                };
                            }).ToList()
                        })
                });

            return Ok(data);
        }



        [HttpPost("create_Reg")]
        public IActionResult create_Reg(AddReg addreg)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid token"
                    });
                }

                var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
                if (student == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Student not found"
                    });
                }

                var registeredCourses = new List<object>();

                foreach (var item in addreg.courses)
                {
                    var course = _context.Courses.FirstOrDefault(w => w.InitialName == item);
                    if (course == null) continue;

                    var academicDetail = new AcadmicDetail()
                    {
                        courseId = course.Id,
                        complete = false,
                        studentId = student.Id,
                        gpa = 0,
                        grade = "",
                        semesterId = int.Parse(addreg.semester)
                    };

                    _context.AcadmicDetails.Add(academicDetail);
                    registeredCourses.Add(new
                    {
                        courseCode = course.InitialName,
                        courseName = course.Name,
                        creditHours = course.Credits
                    });
                }

                _context.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = "Registration completed successfully",
                    studentId = student.Id,
                    semester = addreg.semester,
                    registeredCourses = registeredCourses,
                    totalCourses = registeredCourses.Count,
                   // totalCreditHours = registeredCourses.Sum(c => (int)((dynamic)c).Credits)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred during registration",
                    error = ex.Message
                });
            }
        }

    }
}
