using Application.Services.PhotoService;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services.PhotoService;

public class LocalPhotoService(IWebHostEnvironment webHostEnvironment) : ILocalPhotoService
{
    private IWebHostEnvironment _webHostEnvironment { get; } = webHostEnvironment;

    public string UploadPhoto(IFormFile file, string path)
    {
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, path);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        file.CopyTo(new FileStream(filePath, FileMode.Create));

        return uniqueFileName;
    }
}