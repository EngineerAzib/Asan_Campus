using Asan_Campus.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asan_Campus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SemesterController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetSemester")]
        public IActionResult GetSemester()
        {
            var res=_context.Semesters.ToList();
            return Ok(res);
        }

        [HttpGet("GetSemesterDepartment")]
        public IActionResult GetDepartmentsWithSemesters()
        {
            var result = _context.departmentSemesters
                .Select(ds => new
                {
                    Department = ds.Department.DepartmentName,  // Access DepartmentName via navigation property
                    Semesters = ds.Semester.SemesterName        // Access SemesterName via navigation property
                })
                .ToList();

            return Ok(result); // Return the result as JSON
        }
        [HttpGet("GetSemesterCourse")]
        public IActionResult GetSemesterCourse(int deptId)
        {
            var result = _context.Courses
                .Include(x => x.Semester)  // Include the entire Semester object to access SemesterName
                .Where(x => x.DepartmentId == deptId)  // Filter by the given DepartmentId
                .GroupBy(x => x.Semester)  // Group by Semester (using the entire Semester object)
                .Select(g => new
                {
                    SemesterId = g.Key.Id,  // Access the SemesterId from the grouped Semester object
                    SemesterName = g.Key.SemesterName,  // Access the SemesterName from the grouped Semester object
                    CourseCount = g.Count()  // The number of courses in this semester
                })
                .ToList();  // Get the results as a list

            return Ok(result);  // Return the result as JSON
        }




    }
}
