namespace LabShop.Data;

// One row of the Users table. The property names are the column names.
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "";
}
