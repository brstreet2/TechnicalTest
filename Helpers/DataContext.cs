namespace TechnicalTest.Helpers;

using Microsoft.EntityFrameworkCore;
using TechnicalTest.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration; 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TechnicalTestDatabase"));
    }

    public DbSet<User> Users { get; set; }
}