using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService UserService) : ControllerBase
{
    private IUserService _userService { get; } = UserService;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUserDataAsync();
        return Ok(users);
    }

    [HttpPatch]
    public IActionResult UpdateUser(IFormFile file)
    {
        _userService.UploadUserProfile(file);
        return Ok();
    }
}
