using LabShop.Services;
using Microsoft.Data.Sqlite;

namespace LabShop.Data;

public class Database
{
    private readonly string _connectionString = "Data Source=labshop.db";
    private readonly PasswordService _passwords;

    public Database(PasswordService passwords)
    {
        _passwords = passwords;
    }

    public SqliteConnection Open()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }

    public void Initialize()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                PasswordHash TEXT NOT NULL,
                Role TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT NOT NULL,
                Price REAL NOT NULL
            );
            """;
        cmd.ExecuteNonQuery();

        // Seed demo data only once
        cmd.CommandText = "SELECT COUNT(*) FROM Users";
        if ((long)cmd.ExecuteScalar()! > 0) return;

        // Demo only. In a real app never hard-code passwords.
        var demoUsers = new (string Username, string Password, string Role)[]
        {
            ("admin", "S3cret-demo-pw", "Admin"),
            ("flor",  "demo1234",       "User"),
            ("guest", "guest",          "User"),
        };

        foreach (var (username, password, role) in demoUsers)
        {
            cmd.Parameters.Clear();
            cmd.CommandText =
                "INSERT INTO Users (Username, PasswordHash, Role) " +
                "VALUES ($username, $hash, $role)";
            cmd.Parameters.AddWithValue("$username", username);
            cmd.Parameters.AddWithValue("$hash", _passwords.Hash(password));
            cmd.Parameters.AddWithValue("$role", role);
            cmd.ExecuteNonQuery();
        }

        cmd.Parameters.Clear();
        cmd.CommandText = """
            INSERT INTO Products (Name, Category, Price) VALUES
                ('Mechanical Keyboard',        'Hardware',    89.99),
                ('USB-C Hub',                  'Hardware',    34.50),
                ('27-inch Monitor',            'Hardware',   219.00),
                ('Noise-Cancelling Headphones','Audio',      149.00),
                ('Webcam 1080p',               'Video',       59.90),
                ('Desk Mat',                   'Accessories', 19.99),
                ('Standing Desk',              'Furniture',  329.00),
                ('Cable Organizer Kit',        'Accessories', 12.49);
            """;
        cmd.ExecuteNonQuery();
    }
}
