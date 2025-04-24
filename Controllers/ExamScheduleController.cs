using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asan_Campus.Model.InputClass;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ExamScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetExamScheduleDepartment")]

        public IActionResult GetExamScheduleDepartment()
        {
            var res=_context.Departments.ToDictionary(x=>x.DepartmentCode,x=>x.DepartmentName);
            return Ok(res);
        }

        [HttpGet("GetExamScheduleYear")]
        public IActionResult GetExamScheduleYear(int depId)
        {
            var res = _context.ExamSchedules
                .Where(x => x.DepartmentID == depId)
                .Include(x => x.Year) // Ensure related Year data is loaded
                .ToDictionary(x => x.YearId, x => x.Year.YearName);

            return Ok(res);
        }
        [HttpGet("GetExamScheduleDepYear")]
        public async Task<IActionResult> GetExamScheduleDepYear(string depId, int yearId)
        {
            var examSchedules = await _context.ExamSchedules
                .Where(x => x.Department.DepartmentName == depId && x.YearId == yearId)
                .Include(es => es.Slots) // ✅ Ensure Slots are included
                .ThenInclude(s => s.Course) // ✅ Ensure Course is included
                .Select(es => new
                {
                    date = es.ExamDate.ToString("yyyy-MM-dd"),
                    day = es.ExamDay,
                    slots = es.Slots.Select(slot => new
                    {
                        time = slot.TimeSlot,
                        course = slot.Course.Name,
                        courseCode = slot.Course.InitialName, // Ensure CourseCode exists
                        venue = slot.Venue
                    }).ToList()
                })
                .ToListAsync();

            return Ok(examSchedules);
        }

        [HttpPost("AddExamSchedule")]
        public async Task<IActionResult> AddExamSchedule(AddExamSchedule addExamSchedule)
        {
            var examSchedule = new ExamSchedule()
            {
                 DepartmentID = addExamSchedule.DepartmentID,
                 SemesterID=addExamSchedule.SemesterID,
                 YearId=addExamSchedule.YearId,
                 ExamDate=addExamSchedule.ExamDate,
                 ExamDay=addExamSchedule.ExamDay,

            };
            _context.Add(examSchedule);
            _context.SaveChanges();

            foreach (var item in addExamSchedule.Examslots)
            {
                var examslot = new Slot()
                {
                    CourseId = item.CourseId,
                    TimeSlot = item.TimeSlot,
                    Venue = item.Venue,
                    ExamScheduleId= examSchedule.ScheduleId,
                };
                _context.Add(examslot);
                _context.SaveChanges();
            }
            return Ok("Successfully completed");

        }

        [HttpGet("GetYear")]
        public async Task<IActionResult> GetYear()
        {
            var res = _context.Years.Select(x => new
            {
                x.YearId,
                x.YearName
            });
            return Ok(res);
        }

        [HttpGet("GetStudentExamSchedule")]
        public async Task<IActionResult> GetStudentExamSchedule()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return NotFound("Invalid Token");
            }
            var std = await _context.Students.Where(w => w.UserId == userId).FirstOrDefaultAsync();
            if (std == null) {
                return NotFound("Student Not Found");
              }
            var currentSemesterExams = await _context.ExamSchedules.Where(w => w.SemesterID == std.Semester).Include(w => w.Slots).Select(x => new
            {
                semester = std.Semester,
                department =x.Department.DepartmentName,
                schedule = new
                {
                    date=x.ExamDate,
                    day=x.ExamDay,
                    slot = x.Slots.Select(s=>new
                    {
                        time=s.TimeSlot,
                        course=s.Course.Name,
                        courseCode=s.Course.InitialName,
                        venue=s.Venue,
                        instructor=s.Course.Teacher.Name,
                    })
                }

            }).ToListAsync();
            return Ok(currentSemesterExams);
        }

    }
}
