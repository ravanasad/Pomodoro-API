using Microsoft.AspNetCore.Http;

namespace Application.Services.PhotoService;

public interface ILocalPhotoService
{
    string UploadPhoto(IFormFile file, string path);
}