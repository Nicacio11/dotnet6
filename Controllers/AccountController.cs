
using System.Text.RegularExpressions;
using Blog.Data;
using Blog.DTOs;
using Blog.DTOs.Accounts;
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
    public async Task<ActionResult> CreateAccount([FromServices]EmailService emailService, [FromServices]BlogDataContext context, [FromBody]CreateAccountDTO account)
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
                emailService.Send(new Email
                {
                    ToEmail = user.Email,
                    ToName = user.Name,
                    Subject = "Bem vindo ao Blog",
                    Body = $"Sua senha é {password}"
                });
                return Created($"api/v1/account/{user.Id}", new CreateAccountResultDTO(new CreateAccountDTO(){ Name = user.Name, Email = user.Email }));
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

    [HttpPost("upload-image")]
    [Authorize]
    public async Task<ActionResult> Upload([FromServices]BlogDataContext context, [FromBody]UploadImageDTO dto)
    {
        if(dto is null)
            return BadRequest($"{nameof(dto)} cannot be null.");
        if(!ModelState.IsValid)
            return BadRequest(new CreateAccountResultDTO(ModelState.GetErrors()));

        var fileName = $"{Guid.NewGuid()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(dto.Base64Image, "");

        var bytes = Convert.FromBase64String(data);


        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);

            var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            user.Image = $"images/{fileName}";
            context.Users.Update(user);
            await context.SaveChangesAsync();

        if (user == null)
            return NotFound(new ResultDTO<string>(error: "Usuário não encontrado"));
        }
        catch (System.Exception)
        {
            return StatusCode(500, new ResultDTO<string>(error: "Falha ao inserir imagem"));
        }

        return Ok(new ResultDTO<string>("Imagem alterada com sucesso!", null));
    }
}