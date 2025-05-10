using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    
    // Controllers/DevicesController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDevice([FromBody] DeviceRegistrationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingDevice =  _context.UserDevices
                .FirstOrDefault(d => d.ExpoPushToken == dto.ExpoPushToken);
            var student = _context.Students.Where(w => w.UserId == dto.userId).Select(w=>w.Id).FirstOrDefault();
            if (student == 0)
            {
                return Ok();
            }

            if (existingDevice != null)
            {
                existingDevice.StudentId = student;
                existingDevice.LastUpdated = DateTime.UtcNow;
            }
            else
            {
                var newDevice = new UserDevice
                {
                    ExpoPushToken = dto.ExpoPushToken,
                    StudentId = student
                };
                _context.UserDevices.Add(newDevice);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
