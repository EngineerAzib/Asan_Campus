using Asan_Campus.Model;
using Asan_Campus.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ImplementationSevices _service;
        public NotificationController(ApplicationDbContext context, ImplementationSevices service)
        {
            _context = context;
            _service = service;
        }
        [HttpPost("AddNotification")]
        public IActionResult AddNotification(AddNotification notification)
        {
            try
            {
                var res = new Notification()
                {
                    IsRead = false,
                    Message = notification.Message,
                    Timestamp = notification.Timestamp,
                    date = notification.date,
                    Title = notification.Title,
                    Type = notification.Type,
                    SemesterID = notification.semesterId,
                    DepartmentID = notification.departmentId,
                };

                _context.Notifications.Add(res);
                _context.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = "Notification created successfully",
                    data = res // optional: you can include the created notification
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("GetNotification")]
        public IActionResult GetNotification()
        {
            var res = _context.Notifications.ToList();
            return Ok(res);
        }
        [HttpGet("GetStudentNotification")]
        public IActionResult GetStudentNotification()
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
            var res = _context.Notifications.Where(w=>w.SemesterID== std.Semester && w.DepartmentID==std.DepartmentId).ToList();
            return Ok(res);
        }



    }
}
