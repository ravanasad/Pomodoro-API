using Application.Services.PhotoService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService(UserManager<AppUser> UserManager, IHttpContextAccessor HttpContext, ILocalPhotoService _localPhotoService) : IUserService
{
    private ILocalPhotoService localPhotoService { get; } = _localPhotoService;
    private UserManager<AppUser> _userManager { get; } = UserManager;
    private IHttpContextAccessor _httpContext { get; } = HttpContext;

    public async Task<AppUserDto> GetUserDataAsync()
    {
        string username = _httpContext.HttpContext?.User.Identity?.Name!;
        AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username) ?? throw new Exception("User not found");
        return new(user.UserName!, user.Email!, user.PictureUrl);
    }

    public void UploadUserProfile(IFormFile file)
    {
        string filename = localPhotoService.UploadPhoto(file, "profile-images");
        string username = _httpContext.HttpContext?.User.Identity?.Name!;
        AppUser user = _userManager.Users.FirstOrDefault(x => x.UserName == username) ?? throw new Exception("User not found");
        user.PictureUrl = filename;
        _userManager.UpdateAsync(user);
    }
}

