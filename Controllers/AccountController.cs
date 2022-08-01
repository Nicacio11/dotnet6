
using Blog.Data;
using Blog.DTOs;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

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

    [HttpPost]
    public async Task<ActionResult> CreateAccount([FromServices]BlogDataContext context, [FromBody]CreateAccountDTO account)
    {
        if(account is null)
            return BadRequest($"{nameof(account)} cannot be null.");
        if(!ModelState.IsValid)
            return BadRequest(new CreateAccountResultDTO(ModelState.GetErrors()));

        var password = PasswordGenerator.Generate(25);
        var user = new User 
        {
            Email = account.Email,
            Name = account.Name,
            Slug = account.Name.ToLower(),
            PasswordHash = PasswordHasher.Hash(password)
        };

        try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Created($"api/v1/account/{user.Id}", new CreateAccountResultDTO(new CreateAccountDTO(){ Name = user.Name, Email = user.Email, Password = password }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new CreateAccountResultDTO("Não foi possível inserir um usuário"));
            }
            catch
            {
                return StatusCode(500, new CreateAccountResultDTO("Falha no servidor"));
            }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody]LoginDTO login, [FromServices] BlogDataContext context, [FromServices]TokenService tokenService)
    {
        if(login is null)
            return BadRequest($"{nameof(login)} cannot be null.");
        if(!ModelState.IsValid)
            return BadRequest(new LoginResultDTO(ModelState.GetErrors()));

        var user = await context.Users
        .AsNoTracking()
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == login.Email);

        if(user is null)
            return StatusCode(401, "Invalid user or password.");

        var check = PasswordHasher.Verify(user.PasswordHash, login.Password);
        if(!check)
            return StatusCode(401, "Invalid user or password.");


        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultDTO<string>(token, null));
        }
        catch (System.Exception)
        {
            return StatusCode(500, new LoginResultDTO("Falha interna no servidor"));
        }
    }

    [HttpGet("user")]
    [Authorize(Roles = "user,admin")]
    public IActionResult GetUser()
    {
        return Ok(User.Identity.Name);
    }

    
    [HttpGet("author")]
    [Authorize(Roles = "author")]
    public IActionResult GetAuthor()
    {
        return Ok(User.Identity.Name);
    }

    
    [HttpGet("admin")]
    [Authorize(Roles = "admin")]

    public IActionResult GetAdmin()
    {
        return Ok(User.Identity.Name);
    }
}