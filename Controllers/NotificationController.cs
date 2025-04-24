using Asan_Campus.Model;
using Asan_Campus.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var res = new Notification()
            {
                IsRead = notification.IsRead,
                Message = notification.Message,
                Priority = notification.Priority,
                Timestamp = notification.Timestamp,
                Title = notification.Title,
                Type = notification.Type,
            };
            _context.Notifications.Add(res);
            _context.SaveChanges();
            return Ok("Add Successfully");
        }
        [HttpGet("GetNotification")]
        public IActionResult GetNotification()
        {
            var res = _context.Notifications.ToList();
            return Ok(res);
        }



    }
}
