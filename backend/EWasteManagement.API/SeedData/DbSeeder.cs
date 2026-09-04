using EWasteManagement.API.Data;
using EWasteManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EWasteManagement.API.SeedData
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            try
            {
                // Check if we already have data
                if (await context.Centers.AnyAsync())
                {
                    logger.LogInformation("Database already seeded. Skipping seed.");
                    return;
                }

                logger.LogInformation("Seeding database with sample centers...");

                var centers = GetSampleCenters();
                await context.Centers.AddRangeAsync(centers);
                await context.SaveChangesAsync();

                logger.LogInformation($"{centers.Count} centers seeded successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static List<Center> GetSampleCenters()
        {
            return new List<Center>
            {
                new Center
                {
                    Name = "Colombo Central E-Waste Center",
                    District = "Colombo",
                    Address = "No. 123, Galle Road, Colombo 03",
                    Phone = "0112345678",
                    AcceptedItemType = "Batteries, Electronics, Cables, Mobile Phones",
                    OpeningHours = "8:00 AM - 5:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Kandy Green Recycling Hub",
                    District = "Kandy",
                    Address = "No. 45, Peradeniya Road, Kandy",
                    Phone = "0812234567",
                    AcceptedItemType = "Electronics, Computers, Printers, Cables",
                    OpeningHours = "9:00 AM - 6:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Galle Eco Collection Point",
                    District = "Galle",
                    Address = "No. 78, Lighthouse Street, Galle Fort",
                    Phone = "0912234567",
                    AcceptedItemType = "Batteries, Mobile Phones, Small Electronics",
                    OpeningHours = "8:30 AM - 4:30 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Negombo Waste Recovery Center",
                    District = "Negombo",
                    Address = "No. 234, Beach Road, Negombo",
                    Phone = "0312234567",
                    AcceptedItemType = "Batteries, Electronics, Cables, Plastics",
                    OpeningHours = "7:00 AM - 7:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Jaffna E-Waste Management",
                    District = "Jaffna",
                    Address = "No. 56, Hospital Road, Jaffna",
                    Phone = "0212234567",
                    AcceptedItemType = "Computers, Laptops, Printers, Batteries",
                    OpeningHours = "9:00 AM - 5:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Matara Recycling Center",
                    District = "Matara",
                    Address = "No. 89, Main Street, Matara",
                    Phone = "0412234567",
                    AcceptedItemType = "Electronics, Mobile Phones, Cables",
                    OpeningHours = "8:00 AM - 4:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Kurunegala Tech Recycle",
                    District = "Kurunegala",
                    Address = "No. 12, Colombo Road, Kurunegala",
                    Phone = "0372234567",
                    AcceptedItemType = "Batteries, Electronics, Computers",
                    OpeningHours = "9:30 AM - 5:30 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Ratnapura Green Center",
                    District = "Ratnapura",
                    Address = "No. 34, Gem Street, Ratnapura",
                    Phone = "0452234567",
                    AcceptedItemType = "Batteries, Mobile Phones, Small Electronics",
                    OpeningHours = "8:00 AM - 6:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Badulla E-Waste Hub",
                    District = "Badulla",
                    Address = "No. 67, Welimada Road, Badulla",
                    Phone = "0552234567",
                    AcceptedItemType = "Electronics, Cables, Printers",
                    OpeningHours = "9:00 AM - 4:00 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Anuradhapura Eco Center",
                    District = "Anuradhapura",
                    Address = "No. 23, Sacred City Road, Anuradhapura",
                    Phone = "0252234567",
                    AcceptedItemType = "Batteries, Electronics, Mobile Phones",
                    OpeningHours = "8:30 AM - 5:30 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Polonnaruwa Recycling Point",
                    District = "Polonnaruwa",
                    Address = "No. 45, Tank Road, Polonnaruwa",
                    Phone = "0272234567",
                    AcceptedItemType = "Computers, Electronics, Cables",
                    OpeningHours = "7:30 AM - 6:30 PM",
                    CreatedAt = DateTime.UtcNow
                },
                new Center
                {
                    Name = "Batticaloa Green Initiative",
                    District = "Batticaloa",
                    Address = "No. 78, Kandy Road, Batticaloa",
                    Phone = "0652234567",
                    AcceptedItemType = "Batteries, Mobile Phones, Small Electronics",
                    OpeningHours = "8:00 AM - 5:00 PM",
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}