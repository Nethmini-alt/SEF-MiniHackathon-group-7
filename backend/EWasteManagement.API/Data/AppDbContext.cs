using Microsoft.EntityFrameworkCore;


namespace EWasteManagement.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    

}