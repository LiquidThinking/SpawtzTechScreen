using Microsoft.EntityFrameworkCore;

namespace TechScreen.Web.Data;

public class AppDbContext : DbContext
{
    public DbSet<Fixture> Fixtures => Set<Fixture>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}