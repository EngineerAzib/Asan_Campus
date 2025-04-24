using Asan_Campus.Model;
using Asan_Campus.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ImplementationSevices _implementationSevices;

        public AcademicDetailController(ApplicationDbContext context,ImplementationSevices implementationSevices)
        {
            _context = context;
            _implementationSevices = implementationSevices;
        }
        [HttpPost("AddAcedemicDetail")]
        public IActionResult AddAcedemicDetail(AddAcedemic addAcedemic)
        {
            try
            {
                if (addAcedemic.AcadmicDetail == null || !addAcedemic.AcadmicDetail.Any())
                {
                    return BadRequest(new { success = false, message = "No academic details provided." });
                }

                List<AcadmicDetail> academicDetails = new List<AcadmicDetail>();

                foreach (var item in addAcedemic.AcadmicDetail)
                {
                    var academic = new AcadmicDetail()
                    {
                        semesterId = addAcedemic.semesterId,
                        studentId = addAcedemic.studentId,
                        courseId = item.courseId,
                        gpa = item.gpa,
                        grade = _implementationSevices.Grade(item.gpa),
                        complete = _implementationSevices.Icomplete(item.gpa)
                    };

                    academicDetails.Add(academic);
                }

                _context.AcadmicDetails.AddRange(academicDetails);
                _context.SaveChanges();

                // ✅ Return a JSON response
                return Ok(new { success = true, message = "Successfully Added" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("AddAttendance")]
        public IActionResult AddAttendance(AddAttendance addAttendance)
        {
            foreach (var item in addAttendance.attendanceRecords)
            {
                var attendence = new StudentAttendance()
                {
                     SemesterID=addAttendance.semesterId,
                     StudentID=addAttendance.studentId,
                       CourseID=item.courseId,
                      TotalClasses=item.totalClasses,
                       AttendedClasses=item.attendedClasses,
                };

                _context.StudentAttendances.Add(attendence);
            }
            _context.SaveChanges();

            return Ok(new { success = true, message = "Successfully Added" });
        }




        [HttpGet("GetAcademicDetail")]
        public IActionResult GetAcademicDetail(int stdId)
        {
            var academicDetails = _context.AcadmicDetails.Where(x=>x.studentId== stdId)
                .Include(x => x.semester)
                .Include(x => x.students)
                .Include(x => x.course)
                .ToList();

            var groupedData = academicDetails
                .GroupBy(x => x.semester.SemesterName)
                .Select(g => new
                {
                    semester = g.Key,
                    gpa = g.Average(x => x.gpa), // Calculate average GPA for semester
                    courses = g.Select(x => new
                    {
                        code = x.course.InitialName,
                        name = x.course.Name,
                        creditHours = x.course.Credits,
                        grade = x.grade
                    }).ToList()
                })
                .ToList();

            var response = new
            {
                currentCGPA = academicDetails.Average(x => x.gpa), // Calculate overall CGPA
                semesterGPAs = groupedData
            };

            return Ok(response);
        }
        [HttpGet("GetStudentAcademicDetail")]
        [Authorize]
        public IActionResult GetStudentAcademicDetail()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            // 2. Find student by user ID
            var student = _context.Students
                .FirstOrDefault(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound("Student not found");
            }
            // Get all academic records for the student
            var academicRecords = _context.AcadmicDetails
                .Where(x => x.studentId == student.Id)
                .Include(x => x.semester)
                .Include(x => x.course)
                    .ThenInclude(c => c.Teacher)
                .Include(x => x.course)
                    .ThenInclude(c => c.CourseSchedules)
                .ToList();

            if (!academicRecords.Any())
                return NotFound("No academic records found for this student");

            // Group by semester and transform the data
            var semesters = academicRecords
                .GroupBy(x => x.semester)
                .Select(g => new
                {
                    number = g.Key.Id,
                    // Get the completed status from the first record in the group
                    completed = g.First().complete, // Accessing from AcadmicDetail
                    gpa = g.Average(x => x.gpa),
                    courses = g.Select(x => new
                    {
                        id = x.course.Id,
                        code = x.course.InitialName,
                        name = x.course.Name,
                        creditHours = x.course.Credits,
                        instructor = x.course.Teacher?.Name,
                        
                        grade = x.grade,
                        status = (x.complete ?? false) ? "completed" : "in-progress",
                        progress = (x.complete ?? false) ? 100 : 0,
                        schedule = new
                        {
                            days = x.course.CourseSchedules.FirstOrDefault()?.days?.Split(','),
                            time = $"{x.course.CourseSchedules.FirstOrDefault()?.startTime} - {x.course.CourseSchedules.FirstOrDefault()?.endTime}",
                            room = x.course.CourseSchedules.FirstOrDefault()?.room
                        }
                    }).ToList()
                })
                .OrderBy(x => x.number)
                .ToList();

            // Get current semester (most recent incomplete one)
            var currentSemester = semesters
                .OrderByDescending(s => s.number)
                .FirstOrDefault(s => !s.completed??false);

            // Get all completed semesters
            var allSemesters = semesters
                .Where(s => s.completed??false)
                .OrderBy(s => s.number)
                .ToList();

            // If all semesters are completed, consider the most recent one as current
            if (currentSemester == null && semesters.Any())
            {
                currentSemester = semesters.OrderByDescending(s => s.number).First();
                allSemesters = allSemesters.Where(s => s.number != currentSemester.number).ToList();
            }

            // Prepare the final response
            var response = new
            {
                currentSemester = currentSemester != null ? new
                {
                    number = currentSemester.number,
                    completed = currentSemester.completed,
                    gpa = currentSemester.gpa,
                    courses = currentSemester.courses
                } : null,

                allSemesters = allSemesters.Select(s => new
                {
                    number = s.number,
                    completed = s.completed,
                    gpa = s.gpa,
                    courses = s.courses
                }).ToList()
            };

            return Ok(response);
        }
       
            [HttpGet("student-academics")]
            public async Task<IActionResult> GetStudentAcademics()
            {
                try
                {
                    // Get user ID from token
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Unauthorized("Invalid token");
                    }

                    // Find student
                    var student = await _context.Students
                        .FirstOrDefaultAsync(s => s.UserId == userId);

                    if (student == null)
                    {
                        return NotFound("Student not found");
                    }

                    // Get all academic records
                    var academicRecords = await _context.AcadmicDetails
                        .Where(x => x.students.Id == student.Id)
                        .Include(x => x.semester)
                        .Include(x => x.course)
                        .OrderBy(x => x.semesterId)
                        .ToListAsync();

                    if (!academicRecords.Any())
                    {
                        return NotFound("No academic records found");
                    }

                    // Calculate overall CGPA and total credits
                    var completedRecords = academicRecords.Where(x => x.grade!= null).ToList();
                    var currentCGPA = completedRecords.Any() ? completedRecords.Average(x => x.gpa) : 0;
                    var totalCredits = completedRecords.Sum(x => x.course.Credits);

                    // Group by semester
                    var semesters = academicRecords
                        .GroupBy(x => x.semester)
                        .Select(g => new
                        {
                            semester = g.Key.Id,
                            gpa = g.Average(x => x.gpa),
                            courses = g.Select(x => new
                            {
                                code = x.course.InitialName,
                                name = x.course.Name,
                                creditHours = x.course.Credits,
                                grade = x.grade
                            })
                        })
                        .OrderByDescending(x => x.semester)
                        .ToList();

                    // Get current semester (most recent incomplete one)
                    var currentSemester = semesters.FirstOrDefault(s =>
                        s.courses.Any(c => c.grade == null));

                    // If all semesters are completed, use the most recent as current
                    if (currentSemester == null && semesters.Any())
                    {
                        currentSemester = semesters.First();
                    }

                    // Prepare response
                    var response = new
                    {
                        currentCGPA = Math.Round(currentCGPA, 2),
                        totalCredits,
                        currentSemester = currentSemester != null ? new
                        {
                            semester = currentSemester.semester,
                            courses = currentSemester.courses
                        } : null,
                        semesterGPAs = semesters.Select(s => new
                        {
                            semester = s.semester,
                            gpa = Math.Round(s.gpa, 2),
                            courses = s.courses
                        })
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                  
                    return StatusCode(500, "Internal server error");
                }
            }
        [HttpGet("GetStudentAttendance")]
        public IActionResult GetStudentAttendance()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            // Find student
            var student = _context.Students
                .FirstOrDefault(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound("Student not found");
            }
            var attendanceData = _context.StudentAttendances
                .Where(sa => sa.StudentID == student.Id && sa.Semester.Id == student.Semester)
                .Include(sa => sa.Course)
                .Include(sa => sa.Semester)
                .GroupBy(sa => sa.CourseID) // Group by semester
                .Select(group => new
                {
                    semester = new
                    {

                        OverallAttendance = group.Sum(sa => sa.AttendedClasses) * 100.0 / group.Sum(sa => sa.TotalClasses),
                        Courses = group.Select(sa => new
                        {
                            sa.Course.Id,
                            sa.Course.InitialName,
                            sa.Course.Name,
                            sa.TotalClasses,
                            sa.AttendedClasses,
                            Percentage = sa.TotalClasses == 0 ? 0 : (double)sa.AttendedClasses / sa.TotalClasses * 100
                        }).ToList()
                    }
                })

                .FirstOrDefault();

            return Ok(attendanceData);
        }
            [HttpGet("GetStudentAttendanceByCourse")]
            public IActionResult GetStudentAttendanceByCourse()
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Invalid token");
                }

                var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
                if (student == null)
                {
                    return NotFound("Student not found");
                }

                var attendanceData = _context.StudentAttendances
                    .Where(sa => sa.StudentID == student.Id && sa.Semester.Id == student.Semester)
                    .Include(sa => sa.Course)
                    .GroupBy(sa => sa.Course.Name) // Group by Course Name
                    .Select(group => new
                    {
                        CourseName = group.Key,
                        AttendancePercentage = group.Sum(sa => sa.TotalClasses) == 0
                            ? 0
                            : group.Sum(sa => sa.AttendedClasses) * 100.0 / group.Sum(sa => sa.TotalClasses)
                    })
                    .ToList();

                return Ok(attendanceData);
            }


        }



    }

    


