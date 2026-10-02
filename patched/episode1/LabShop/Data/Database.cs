using Microsoft.Data.Sqlite;

namespace LabShop.Data;

public class Database
{
    private readonly string _connectionString = "Data Source=labshop.db";

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
                Password TEXT NOT NULL,
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

        cmd.CommandText = """
            INSERT INTO Users (Username, Password, Role) VALUES
                ('admin', 'S3cret-demo-pw', 'Admin'),
                ('flor',  'demo1234',       'User'),
                ('guest', 'guest',          'User');

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
