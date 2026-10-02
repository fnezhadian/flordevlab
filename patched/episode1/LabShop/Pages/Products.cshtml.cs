using LabShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace LabShop.Pages;

public class ProductsModel : PageModel
{
    private readonly Database _db;

    public ProductsModel(Database db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public List<(string Name, string Category, string Price)> Results { get; } = new();

    public string? Error { get; set; }

    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Search)) return;

        try
        {
            using var conn = _db.Open();
            using var cmd = conn.CreateCommand();

            // VULNERABLE: the search text is glued straight into the SQL text
            cmd.CommandText =
                "SELECT Name, Category, Price FROM Products " +
                "WHERE Name LIKE '%" + Search + "%'";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Results.Add((
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetValue(2).ToString() ?? ""));
            }
        }
        catch (SqliteException ex)
        {
            // Also a problem: this shows raw database errors to the visitor
            Error = ex.Message;
        }
    }
}
