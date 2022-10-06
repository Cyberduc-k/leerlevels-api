using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controller.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly UserService userService;
    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

}
