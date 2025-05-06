using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("AdminDashboard")]
        public IActionResult AdminDashboard()
        {
            // Dashboard Summary
            int totalStudents = _context.Students.Count();
            int totalTeachers = _context.Teachers.Count();
            int totalDepartments = _context.Departments.Count();
            int totalCourses = _context.Courses.Count();
            int pendingRequests = _context.Notifications.Count(n => !n.IsRead); // Assuming unread means pending

            // Gender Distribution
            var genderTotals = new
            {
                male = _context.Departments.Sum(d => d.MaleStudent),
                female = _context.Departments.Sum(d => d.FemaleStudent)
            };

            // Department Strength (Example: name, total students, color)
            var departmentStrength = _context.Departments.Select(d => new
            {
                name = d.DepartmentName,
                students = d.MaleStudent + d.FemaleStudent,
                color = "#" + Guid.NewGuid().ToString("N").Substring(0, 6) // Random color (you can use your own logic)
            }).ToList();

            // Attendance Stats (dummy logic - adjust according to your structure)
            var attendance = new
            {
                present = 87.2,
                absent = 8.5,
                leave = 4.3
            };

            // Upcoming Exam Schedules
            var examSchedules = _context.ExamSchedules
                .Where(e => e.ExamDate >= DateTime.Now)
                .Select(e => new
                {
                    id = e.ScheduleId,
                    department = e.Department.DepartmentName,
                    year = e.Year,
                    date = e.ExamDate.ToString("yyyy-MM-dd"),
                    status = "Upcoming"
                }).Take(5)
                .ToList();

            // Semester Registrations
            var semesterRegistrations = _context.SemesterRegistration.Select(sr => new
            {
                id = sr.id,
                department = sr.Department.DepartmentName,
                semester = sr.Semester.SemesterName,
                status = "open",
                courses = 1,
                students = sr.MaxStudent
            }).Take(5).ToList();

            // Recent Activities (example: logs or actions)
            //var recentActivities = _context.Activities
            //    .OrderByDescending(a => a.Timestamp)
            //    .Take(5)
            //    .Select(a => new
            //    {
            //        id = a.Id,
            //        type = a.Type,
            //        action = a.Action,
            //        details = a.Details,
            //        time = a.Timestamp.ToString("g")
            //    })
            //    .ToList();

            // Notifications
            var recentActivities = _context.Notifications
                .OrderByDescending(n => n.Timestamp)
                .Take(4)
                .Select(n => new
                {
                    id = n.Id,
                    type="course",
                    action = n.Title,
                    details = n.Message,
                    time = n.Timestamp.ToString("g"),
                    //read = n.IsRead
                })
                .ToList();

            // Performance Metrics (placeholder values - you can replace with real logic)
            var performanceMetrics = new
            {
                weekly = new
                {
                    labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    datasets = new[]
                    {
                new {
                    data = new[] { 65, 72, 84, 78, 90, 82, 76 },
                    color = "#4F46E5"
                }
            }
                },
                monthly = new
                {
                    labels = new[] { "Week 1", "Week 2", "Week 3", "Week 4" },
                    datasets = new[]
                    {
                new {
                    data = new[] { 78, 82, 85, 89 },
                    color = "#10B981"
                }
            }
                },
                yearly = new
                {
                    labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                    datasets = new[]
                    {
                new {
                    data = new[] { 65, 68, 72, 75, 78, 82, 85, 89, 92, 90, 88, 91 },
                    color = "#8B5CF6"
                }
            }
                }
            };

            // Return everything as a single object
            var dashboardData = new
            {
                totalStudents,
                totalTeachers,
                totalDepartments,
                totalCourses,
                pendingRequests,
                studentGenderDistribution = genderTotals,
                departmentStrength,
                attendance,
                examSchedules,
                semesterRegistrations,
                recentActivities,
             

                performanceMetrics
            };

            return Ok(dashboardData);
        }
        [HttpGet("StudentDashboard")]
        public IActionResult StudentDashboard()
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

            var academicDetails = _context.AcadmicDetails
      .Where(w => w.studentId == student.Id)
      .Include(w => w.course)
      .ToList();

            if (!academicDetails.Any())
            {
                return NotFound("No academic data found");
            }

            var latestSemester = academicDetails
                .OrderByDescending(x => x.semesterId)
                .FirstOrDefault();

            var completedCredits = academicDetails
                .Where(x => x.complete == true)
                .Sum(x => x.course.Credits);

            var totalCredits = academicDetails
                .Sum(x => x.course.Credits);

            var courseList = academicDetails.Where(w=>w.complete==false).Select(x => new
            {
                code = x.course.InitialName,
                name = x.course.Name,
                status = x.complete == true ? "Complete" : "Not Complete"
            }).ToList();
            var result = new
            {
                semester = latestSemester?.semesterId ?? 0,
                year = latestSemester.students.RollNo,
                progress = (int)Math.Round(((double)completedCredits / (totalCredits == 0 ? 1 : totalCredits)) * 100),
                completedCredits = completedCredits,
                totalCredits = totalCredits,
                courses = courseList
            };

            return Ok(result);

        }
        [HttpGet("upcoming-events")]
        public IActionResult GetUpcomingEvents()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var upcomingEvents = _context.Events
                .Where(e => e.StartDate >= today)
                .OrderBy(e => e.StartDate)
                .Take(5)
                .Select(e => new
                {
                    type = e.Name,
                    title = e.Description,
                    time = e.StartDate.ToString("dddd, MMM dd") // e.g., Friday, Apr 25
                })
                .ToList();

            return Ok(upcomingEvents);
        }

        [HttpGet("attendance-summary")]
        public IActionResult GetAttendanceSummary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");

            var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
            if (student == null)
                return NotFound("Student not found");

            var attendanceRecords = _context.StudentAttendances
                .Where(a => a.StudentID == student.Id && a.SemesterID == student.Semester)
                .ToList();

            var totalClasses = attendanceRecords.Count();
            var presentDays = attendanceRecords.Count(a => a.AttendedClasses > 0);

            var overallPercentage = totalClasses == 0 ? 0 : (presentDays * 100) / totalClasses;

            // Weekly trend - You can update this logic based on your real data
            var weeklyTrend = new List<int> { 75, 80, 85, 82 };

            var result = new
            {
                overallPercentage,
                presentDays,
                totalClasses,
                weeklyTrend
            };

            return Ok(result);
        }
        [HttpGet("GetProfileInfo")]
        public IActionResult GetProfileInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            object userInfo = null;

            if (User.IsInRole("ADMIN"))
            {
                userInfo = _context.Users
                    .Where(w => w.Id == userId)
                    .Select(w => new
                    {
                        AdminID= "ADMIN-001",
                        Email=w.Email,
                        Phone="03303189922",
                       
                    })
                    .FirstOrDefault();
            }
            else if (User.IsInRole("STUDENT"))
            {
                userInfo = _context.Students
                    .Include(s => s.User)  // Load the related User data
                    .Where(s => s.UserId == userId)
                    .Select(s => new
                    {
                        AdminID = s.EndrollementNo,
                        Email = s.User.Email,
                        Phone =s.Phone,// Access email from the included User
                      
                    })
                    .FirstOrDefault();
            }
            if (userInfo == null)
            {
                return NotFound("User not found");
            }

            return Ok(userInfo);
        }



    }
}
