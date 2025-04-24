using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using static Asan_Campus.Model.InputClass;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterenshipController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public InterenshipController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("AddInternship")]
        public IActionResult AddInternship(AddInternship request)
        {
            
            var internship = new Internship()
            {
                Title=request.Title,
                CompanyName=request.CompanyName,
                departmentId=request.departmentId,
                Stippend=request.Stippend,
                Position=request.Position,
                Year=request.Year,

                Location=request.Location,
                IsActive=true,
                ApplicationDeadline=request.ApplicationDeadline,
                Duration=request.Duration, 
                
                PostedDate= DateTime.Now,
            };
            _context.Add(internship);
            _context.SaveChanges();
            return Ok("Successfully Added");

        }
        [HttpGet("GetInternship")]
        public IActionResult GetInternship()
        {
            var res = _context.Internships.Select(
                x => new
                {
                    x.Id,
                    x.Location,
                    ApplicationDeadline = x.ApplicationDeadline.ToString("yyyy-MM-dd"),
                    PostedDate= x.PostedDate.ToString("yyyy-MM-dd"),
                    x.Position,
                    x.Title, x.CompanyName,
                    x.Stippend,x.IsActive,
                    x.Duration,
                    x.Year,
                    x.departmentId,
                    DepartmentTitle=x.department.DepartmentName
                });
            return Ok(res);

        }
        [HttpGet("GetStudentInternship")]
        public async Task<IActionResult> GetStudentInternship()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value ?? null;
            if (userId == null)
            {
                return NotFound("Token Not Found");

            }
            var std = _context.Students.Where(x => x.UserId == userId).FirstOrDefault();
            if (std == null)
            {
                return NotFound("User Not Found");
            }
            var internships = _context.Internships.Where(x => x.departmentId == std.DepartmentId).Select(x=>new
            {
             company=x.CompanyName,
            title=x.Title,
            status = x.IsActive ?? true ? "Active" : "InActive",
             location=x.Location,
            duration=x.Duration,
            stipend=x.Stippend,
            positions=x.Position,
            //requirements=x.,
            deadline=x.ApplicationDeadline,
            postedDate=x.PostedDate,
            
            }).ToList();

            return Ok(internships);
        }


    }
}
