using LabShop.Services;
using Microsoft.Data.Sqlite;

namespace LabShop.Data;

/// <summary>
/// One-time migration: plain text passwords -> hashes.
///   1) add the PasswordHash column
///   2) hash every existing password
///   3) drop the old plain text Password column
/// Steps 2 and 3 run in ONE transaction. If anything fails, nothing changes.
/// BACK UP THE DATABASE FIRST.
/// </summary>
public static class PasswordMigration
{
    public static int Run(Database db, PasswordService passwords)
    {
        using var conn = db.Open();

        // Nothing to migrate (already done, or a brand new database)
        if (!ColumnExists(conn, "Password")) return 0;

        // 1) new column
        if (!ColumnExists(conn, "PasswordHash"))
            Execute(conn, null, "ALTER TABLE Users ADD COLUMN PasswordHash TEXT NOT NULL DEFAULT ''");

        // read the plain text passwords that still need hashing
        var rows = new List<(int Id, string Plain)>();
        using (var read = conn.CreateCommand())
        {
            read.CommandText = "SELECT Id, Password FROM Users WHERE PasswordHash = ''";
            using var reader = read.ExecuteReader();
            while (reader.Read())
                rows.Add((reader.GetInt32(0), reader.GetString(1)));
        }

        // 2) + 3) hash everything, check, then drop the plain text column
        using (var tx = conn.BeginTransaction())
        {
            foreach (var (id, plain) in rows)
            {
                using var update = conn.CreateCommand();
                update.Transaction = tx;
                update.CommandText = "UPDATE Users SET PasswordHash = $hash WHERE Id = $id";
                update.Parameters.AddWithValue("$hash", passwords.Hash(plain));
                update.Parameters.AddWithValue("$id", id);
                update.ExecuteNonQuery();
            }

            // Safety check: never drop the old column if any user was left unhashed
            using (var check = conn.CreateCommand())
            {
                check.Transaction = tx;
                check.CommandText = "SELECT COUNT(*) FROM Users WHERE PasswordHash = ''";
                if ((long)check.ExecuteScalar()! > 0)
                    throw new InvalidOperationException("Some users were not hashed. Nothing was changed.");
            }

            Execute(conn, tx, "ALTER TABLE Users DROP COLUMN Password");
            tx.Commit();
        }

        // Dropping a column can leave old data behind in unused pages of the file.
        // VACUUM rebuilds the file, so the plain text passwords are really gone.
        Execute(conn, null, "VACUUM");

        return rows.Count;
    }

    private static bool ColumnExists(SqliteConnection conn, string column)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Users') WHERE name = $name";
        cmd.Parameters.AddWithValue("$name", column);
        return (long)cmd.ExecuteScalar()! > 0;
    }

    private static void Execute(SqliteConnection conn, SqliteTransaction? tx, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
