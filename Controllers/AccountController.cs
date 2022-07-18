
using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly TokenService tokenService;

    public AccountController(
        ILogger<AccountController> logger,
        TokenService tokenService)
    {
        _logger = logger;
        this.tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var token = tokenService.GenerateToken(null); 

        return Ok(token);
    }
}