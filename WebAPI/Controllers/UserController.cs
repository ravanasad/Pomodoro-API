namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


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
