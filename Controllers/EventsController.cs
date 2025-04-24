using Asan_Campus.Helper;
using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asan_Campus.Model.InputClass;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("AddEvents")]
        public IActionResult AddEvents([FromForm] AddEvent? request,IFormFile? image) 
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            string imageUrl = null;

            // Upload the image and get the file path
            if (image != null)
            {
                try
                {
                    imageUrl = ImageHelper.UploadImage(image, folderPath);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = "Image upload failed", Error = ex.Message });
                }
            }

            var publishedStartDate = new DateOnly(
              request.StartDate.Year,
              request.StartDate.Month,
              request.EndDate.Day
       );

            // Convert TimeDto to TimeOnly
            var publishedStartTime = new TimeOnly(
                request.StartTime.Hour,
                request.StartTime.Minute
            );
            var publishedEndDate = new DateOnly(
             request.StartDate.Year,
             request.StartDate.Month,
             request.EndDate.Day
      );

            // Convert TimeDto to TimeOnly
            var publishedEndTime = new TimeOnly(
                request.StartTime.Hour,
                request.StartTime.Minute
            );

            var events = new Event()
            {
                Name = request.Name,
                Description = request.Description,
                StartDate= publishedStartDate,
                EndDate= publishedEndDate,
                Location = request.Location,
                StartTime = publishedStartTime,
                EndTime = publishedEndTime,
                ImageUrl = imageUrl,
                EventCategoryId=request.EventCategoryId,
                _isActive =true
            };
            _context.Events.Add(events);
            _context.SaveChanges();
            return Ok("Event created successfully");
        }
        [HttpGet("GetEvent")]
        public IActionResult GetEvent()
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value ?? null;
            //if (userId == null)
            //{
            //    return NotFound("Token Not Found");

            //}
          
            var res = _context.Events.Include(x => x.EventCategory).Select(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Location,
                    x.StartDate,
                    x.EndDate,
                    x.StartTime,
                    x.EndTime,
                    x.ImageUrl,
                    x._isActive,
                    CategoryTitle = x.EventCategory.Title
                }
                ).ToList(); 
            return Ok(res);
        }

        [HttpGet("GetStudentEvent")]
        public IActionResult GetStudentEvent()
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value ?? null;
            //if (userId == null)
            //{
            //    return NotFound("Token Not Found");

            //}

            var res = _context.Events.Include(x => x.EventCategory).Select(
                x => new
                {
                    x.Id,
                   title= x.Name,
                    x.Description,
                    venue= x.Location,
                    date=  x.StartDate,
                    x.EndDate,
                    time=x.StartTime,
                    x.EndTime,
                    image= x.ImageUrl,
                    x._isActive,
                    eventType = x.EventCategory.Title
                }
                ).ToList();
            return Ok(res);
        }

    }
}
