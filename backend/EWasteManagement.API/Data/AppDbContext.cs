using Microsoft.EntityFrameworkCore;
using EWasteManagement.API.Models;

namespace EWasteManagement.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Center> Centers { get; set; }

}