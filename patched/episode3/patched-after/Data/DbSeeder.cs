using LabShop.Services;

namespace LabShop.Data;

// Demo data only, same rows as before. Runs only when a table is empty.
public static class DbSeeder
{
    public static void Seed(LabDbContext db, PasswordService passwords)
    {
        if (!db.Users.Any())
        {
            // Demo only. In a real app never hard-code passwords.
            db.Users.AddRange(
                new User { Username = "admin", PasswordHash = passwords.Hash("S3cret-demo-pw"), Role = "Admin" },
                new User { Username = "flor",  PasswordHash = passwords.Hash("demo1234"),       Role = "User" },
                new User { Username = "guest", PasswordHash = passwords.Hash("guest"),          Role = "User" });
        }

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Mechanical Keyboard",         Category = "Hardware",    Price = 89.99 },
                new Product { Name = "USB-C Hub",                   Category = "Hardware",    Price = 34.50 },
                new Product { Name = "27-inch Monitor",             Category = "Hardware",    Price = 219.00 },
                new Product { Name = "Noise-Cancelling Headphones", Category = "Audio",       Price = 149.00 },
                new Product { Name = "Webcam 1080p",                Category = "Video",       Price = 59.90 },
                new Product { Name = "Desk Mat",                    Category = "Accessories", Price = 19.99 },
                new Product { Name = "Standing Desk",               Category = "Furniture",   Price = 329.00 },
                new Product { Name = "Cable Organizer Kit",         Category = "Accessories", Price = 12.49 });
        }

        db.SaveChanges();
    }
}
