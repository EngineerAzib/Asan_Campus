using Asan_Campus.Helper;
using Asan_Campus.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using static Asan_Campus.Model.InputClass;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageHelper _imageHelper;
        public NewsController(ApplicationDbContext context, ImageHelper imageHelper )
        {
            _context = context;
            _imageHelper = imageHelper;
        }
        [HttpPost("AddNews")]
        public IActionResult AddNews([FromForm] AddNew? request, IFormFile? image)
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

            var publishedDate = new DateOnly(
           request.PublishedDate.Year,
           request.PublishedDate.Month,
           request.PublishedDate.Day
       );

            // Convert TimeDto to TimeOnly
            var publishedTime = new TimeOnly(
                request.PublishedTime.Hour,
                request.PublishedTime.Minute
            );
            var news = new New()
            {
                Title = request.Title,
                Description=request.Description,
                PublishedDate = publishedDate,
                PublishedTime = publishedTime,
                CategoryId=request.categoryId,
                ImageUrl = imageUrl, // Save the image file name or URL
                _isActive = true
            };
            _context.Add(news);
            _context.SaveChanges();
            return Ok("News Add Successfully");
        }
        [HttpGet("GetNews")]
        public IActionResult getNews()
        {
            var res = _context.News
                .Include(x => x.Category)  // Include the related Category entity
                .Select(x => new {
                    x.Id,
                    x.Title,
                    x.Description,
                    x.PublishedDate,
                    x.PublishedTime,
                    x.ImageUrl,
                    CategoryTitle = x.Category.Title // Access the Title from the related Category entity
                })
                .ToList();
            return Ok(res);
        }
        [HttpGet("GetStudentNews")]
        public IActionResult GetStudentNews()
        {
            var res = _context.News
                .Include(x => x.Category)  // Include the related Category entity
                .Select(x => new {
                    x.Id,
                    x.Title,
                    content=x.Description,
                    publishDate= x.PublishedDate,
                    x.PublishedTime,
                    x.ImageUrl,
                    category = x.Category.Title // Access the Title from the related Category entity
                })
                .ToList();
            return Ok(res);
        }
        [HttpGet("GetNewsCategory")]
        public IActionResult GetNewsCategory()
        {
            var res = _context.Categories.ToList();
            return Ok(res);
        }


    }
}
