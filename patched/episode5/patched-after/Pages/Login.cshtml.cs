using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using LabShop.Data;
using LabShop.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;   // NEW (Ep 5)
using Microsoft.EntityFrameworkCore;

namespace LabShop.Pages;

// NEW (Ep 5): the one line. Apply the "login" rate limit to this page.
[EnableRateLimiting("login")]
public class LoginModel : PageModel
{
    private readonly LabDbContext _db;
    private readonly PasswordService _passwords;

    public LoginModel(LabDbContext db, PasswordService passwords)
    {
        _db = db;
        _passwords = passwords;
    }

    // NEW (Ep 4): allow list rules. Only these characters, only this long.
    // Wrong characters get the same generic message as a wrong password (no hints for an attacker).
    [BindProperty]
    [Required(ErrorMessage = "Enter your username.")]
    [StringLength(50, ErrorMessage = "Username is too long.")]
    [RegularExpression(@"^[A-Za-z0-9._-]+$", ErrorMessage = "Invalid username or password.")]
    public string Username { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Enter your password.")]
    [StringLength(128, ErrorMessage = "Password is too long.")]
    public string Password { get; set; } = "";

    public string? Message { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        // NEW (Ep 4): the one line that matters. Not valid? Stop here.
        // Nothing below runs, so the database never sees this request.
        if (!ModelState.IsValid) return Page();

        var username = Username ?? "";

        // CHANGED (Ep 3): EF instead of a hand-written query.
        // Still username ONLY. The typed password never touches the query.
        // Only the columns we need, same as the old SELECT.
        var user = await _db.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .Select(u => new { u.Username, u.Role, u.PasswordHash })
            .FirstOrDefaultAsync();

        // UNCHANGED from Ep 2: Verify runs even when the user does not exist (hash is null),
        // so the response time does not reveal which usernames are real.
        var passwordOk = _passwords.Verify(user?.PasswordHash, Password ?? "");

        if (!passwordOk || user == null)
        {
            // Same message for a wrong username and a wrong password
            Message = "Invalid username or password.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToPage("/Index");
    }
}
