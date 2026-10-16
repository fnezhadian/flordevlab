using System.Security.Claims;
using LabShop.Data;
using LabShop.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabShop.Pages;

public class LoginModel : PageModel
{
    private readonly Database _db;
    private readonly PasswordService _passwords;

    public LoginModel(Database db, PasswordService passwords)
    {
        _db = db;
        _passwords = passwords;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public string? Message { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        string? name = null;
        string? role = null;
        string? hash = null;

        using (var conn = _db.Open())
        using (var cmd = conn.CreateCommand())
        {
            // PATCHED (Ep 2): find the user by username ONLY.
            // The typed password never touches the SQL.
            cmd.CommandText =
                "SELECT Username, Role, PasswordHash FROM Users " +
                "WHERE Username = $username";
            cmd.Parameters.AddWithValue("$username", Username ?? "");

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                name = reader.GetString(0);
                role = reader.GetString(1);
                hash = reader.GetString(2);
            }
        }

        // Verify runs even when the user does not exist (hash is null),
        // so the response time does not reveal which usernames are real.
        var passwordOk = _passwords.Verify(hash, Password ?? "");

        if (!passwordOk || name == null || role == null)
        {
            // Same message for a wrong username and a wrong password
            Message = "Invalid username or password.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, name),
            new(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToPage("/Index");
    }
}
