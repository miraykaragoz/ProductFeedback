using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductFeedback.Model;

namespace ProductFeedback.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpPost("/Register")]
    public async Task<IActionResult> Register(Register model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Nickname = model.Nickname,
        };

        if (model.Image != null && model.Image.Length > 0)
        {
            var photoPath = await SaveProfilePhoto(model.Image);
            
            if (photoPath == null)
            {
                return BadRequest( new { message = "Geçersiz fotoğraf." });
            }
            
            user.ImagePath = photoPath;
        }
        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok("Kullanıcı kaydedildi.");
        }
        
        return BadRequest(result.Errors);
    }

    [HttpPost("/Login")]
    public async Task<IActionResult> Login(Login model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok("Giriş yapıldı.");
        }
            
        return Unauthorized("Bu kullanıcı bulunamadı.");
    }
    
    [HttpPost("/Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Çıkış yapıldı.");
    }
    
    [Authorize]
    [HttpGet("/Me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("Kullanıcı girişi yapılmamış.");
        }

        var userProfile = new
        {
            user.FirstName,
            user.LastName,
            user.Fullname,
            user.Email
        };
        
        return Ok(userProfile);
    }
    
    private async Task<string> SaveProfilePhoto(IFormFile photo)
    {
        var photoName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
        
        var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", photoName);

        try
        {
            using var stream = new FileStream(photoPath, FileMode.Create);
            await photo.CopyToAsync(stream);
        
            return Path.Combine("uploads", photoName);
        }
        catch (Exception)
        {
            return null; 
        }
    }
}