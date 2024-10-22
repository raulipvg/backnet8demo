using AngularApp4.Server;
using AngularApp4.Server.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pais> Pais { get; set; }
    public DbSet<Cliente> Cliente { get; set; }

   
}