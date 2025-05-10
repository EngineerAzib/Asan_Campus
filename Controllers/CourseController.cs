using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("add-course")]
        public IActionResult AddCourse([FromBody] AddCourseRequest request)
        {
            // Validate semester and department
            if (!_context.Semesters.Any(s => s.Id == request.SemesterId))
                return BadRequest("Invalid Semester ID");
            if (!_context.Departments.Any(d => d.Id == request.DepartmentId))
                return BadRequest("Invalid Department ID");

            // Create the course
            var course = new Course
            {
                Name = request.Name,
                Credits = request.Credits,
                isElective=request.isElective,
                DepartmentId = request.DepartmentId,
                SemesterId = request.SemesterId,
                InitialName=request.InitialName,
                TeacherId=request.teacherId,
                type=request.type
                //startDate = request.startDate,
                //endDate = request.endDate,
                //days =request.days
            };
            _context.Courses.Add(course);
            _context.SaveChanges();
            var courseSchedule = new CourseSchedule
            {
                 courseId=course.Id,
                  days= request.day,
                startTime = "empty",
                endTime = "empty",
                room = "empty"
            };
            _context.CourseSchedules.Add(courseSchedule);
            _context.SaveChanges();
            // Add prerequisites if any
            if (request.PrerequisiteIds != null && request.PrerequisiteIds.Any())
            {
                foreach (var prereqId in request.PrerequisiteIds)
                {
                    if (!_context.Courses.Any(c => c.Id == prereqId))
                        return BadRequest($"Invalid Prerequisite Course ID: {prereqId}");

                    var prerequisite = new Prerequisite
                    {
                        CourseId = course.Id,
                        PrerequisiteCourseId = prereqId
                    };
                    _context.Prerequisites.Add(prerequisite);
                }
                _context.SaveChanges();
            }

            return Ok(new { message = "Course added successfully", courseId = course.Id });
        }

        [HttpGet("GetCourse")]
        public IActionResult GetCourse(int depId, int semId)
        {
            var res=_context.Courses.Include(x=>x.Teacher).Where(x=>x.DepartmentId== depId && x.SemesterId== semId).Select(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Credits,
                    x.InitialName,
                    TeacherName=x.Teacher.Name,
                    

                }).ToList();
            return Ok(res);
        }
        [HttpGet("GetAllTeacher")]
        public IActionResult GetAllTeacher()
        {
            var res = _context.Teachers.Select(x => new
            {
                x.Id,
                x.Name,
               
            }).ToList();
            return Ok(res);
        }
        [HttpGet("GetAllCourse")]
        public IActionResult GetAllCourse()
        {
            var res = _context.Courses.Select(
                x => new
                {
                    x.Id,
                    x.Name
                    
                  


                }).ToList();
            return Ok(res);
        }
        [HttpGet("GetCoursebyStudent")]
        public IActionResult GetAllCourse(int studentId)
        {
            int semesterId=_context.Students.Where(x=>x.Id==studentId).Select(x=>x.Semester).FirstOrDefault();
            var res = _context.AcadmicDetails.Include(w=>w.course).Where(x=>x.studentId==studentId && x.semesterId==semesterId && x.complete==false).Select(
                x => new
                {
                    x.course.Id,
                    x.course.Name




                }).ToList();
            return Ok(res);
        }


    }
}
