using System.Security.Claims;
using LabShop.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabShop.Pages;

public class LoginModel : PageModel
{
    private readonly Database _db;

    public LoginModel(Database db)
    {
        _db = db;
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

        using (var conn = _db.Open())
        using (var cmd = conn.CreateCommand())
        {
            // VULNERABLE: user input is glued straight into the SQL text
            cmd.CommandText =
                "SELECT Username, Role FROM Users " +
                "WHERE Username = '" + Username + "' AND Password = '" + Password + "'";

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                name = reader.GetString(0);
                role = reader.GetString(1);
            }
        }

        if (name == null || role == null)
        {
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
