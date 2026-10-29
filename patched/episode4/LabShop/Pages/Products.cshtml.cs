using System.Data.Common;
using LabShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabShop.Pages;

public class ProductsModel : PageModel
{
    private readonly LabDbContext _db;

    public ProductsModel(LabDbContext db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public List<Product> Results { get; } = new();

    public string? Error { get; set; }

    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Search)) return;

        try
        {
            // EF turns this into SQL and sends the pattern as a parameter.
            // EF.Functions.Like keeps the old LIKE behavior (Contains() would be case-sensitive on SQLite).
            var pattern = "%" + Search + "%";

            Results.AddRange(
                _db.Products
                    .AsNoTracking()
                    .Where(p => EF.Functions.Like(p.Name, pattern))
                    .ToList());
        }
        catch (DbException)
        {
            // PATCHED (Ep 1): never show raw database errors to the visitor
            Error = "Something went wrong. Please try again.";
        }
    }
}
