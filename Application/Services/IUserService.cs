using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface IUserService
{
    Task<AppUserDto> GetUserDataAsync();
    void UploadUserProfile(IFormFile file);
}

