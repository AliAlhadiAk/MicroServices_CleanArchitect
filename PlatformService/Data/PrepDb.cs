using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    // Apply any pending migrations
                    context.Database.Migrate();
                    Console.WriteLine("--> Migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }

                // Seed data if necessary
                SeedData(context, isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            // Check if there are any existing platforms in the database
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                // Add some initial data
                context.Platforms.AddRange(
                    new Models.Platfrom { Id = 1,Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Models.Platfrom { Id = 2,Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Models.Platfrom { Id = 3,Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

                context.SaveChanges();
                Console.WriteLine("--> Data seeded successfully.");
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
