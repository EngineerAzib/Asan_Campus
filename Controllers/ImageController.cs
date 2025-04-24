using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asan_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet("download-file/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            // Define the directory where images and PDFs are stored
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Return 404 if the file does not exist
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileExtension = Path.GetExtension(fileName).ToLower();

            string contentType;
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".pdf":
                    contentType = "application/pdf"; // MIME type for PDF
                    break;
                default:
                    contentType = "application/octet-stream"; // Fallback for unknown file types
                    break;
            }

            return File(fileBytes, contentType, fileName); // This will trigger the download
        }
    }
}
