namespace Asan_Campus.Helper
{
    public class ImageHelper
    {
        public static string UploadImage(IFormFile image, string folderPath)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("Invalid image file.");
            }

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique file name
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";

            // Combine the folder path and file name
            var filePath = Path.Combine(folderPath, fileName);

            // Save the image to the folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return fileName; // Return the file name or URL
        }
    }
}
