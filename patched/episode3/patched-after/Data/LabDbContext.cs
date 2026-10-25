using Microsoft.EntityFrameworkCore;

namespace LabShop.Data;

// The one door to the database. Replaces the hand-written Database class.
public class LabDbContext : DbContext
{
    public LabDbContext(DbContextOptions<LabDbContext> options) : base(options)
    {
    }

    // DbSet property names = table names (Users, Products), same as before.
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
}
