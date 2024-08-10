using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Repos.Caching;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Data.Repos
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlatformRepo> _logger;
        private readonly ICacheService _cacheService;

        public PlatformRepo(AppDbContext context, ILogger<PlatformRepo> logger, ICacheService cacheService)
        {
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task CreatePlatform(Models.Platfrom platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            await _context.Platforms.AddAsync(platform);
            var changesSaved = await _context.SaveChangesAsync();

            if (changesSaved > 0)
            {
                var platforms = await _context.Platforms.ToListAsync();
                _cacheService.UpdateCacheIfExists<IEnumerable<Models.Platfrom>>("all_platforms", platforms, TimeSpan.FromMinutes(2));
            }
            else
            {
                _logger.LogWarning("No changes were saved when creating platform.");
            }
        }

        public async Task<bool> DeletePlatform(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine($"Invalid ID: {id}");
                return false;
            }

            Console.WriteLine($"Attempting to delete platform with ID: {id}");

            var platform = await _context.Platforms.FirstOrDefaultAsync(x => x.Id == id);

            if (platform == null)
            {
                Console.WriteLine($"Platform with ID {id} not found.");
                return false;
            }

            Console.WriteLine($"Platform found: {platform.Name}");

            _context.Platforms.Remove(platform);

            await _context.SaveChangesAsync();

            _cacheService.RemoveData($"platform_{id}");

            var platforms = await _context.Platforms.ToListAsync();
            _cacheService.UpdateCacheIfExists<IEnumerable<Models.Platfrom>>("all_platforms", platforms, TimeSpan.FromMinutes(2));

            Console.WriteLine($"Platform with ID {id} deleted successfully.");
            return true;
        }


        public async Task<IEnumerable<Models.Platfrom>> GetAllPlatforms()
        {
            const string cacheKey = "all_platforms";
            const int cacheDurationInMinutes = 10;

            var cachedPlatforms = _cacheService.GetData<IEnumerable<Models.Platfrom>>(cacheKey);

            if (cachedPlatforms != null && cachedPlatforms.Any())
            {
                Console.WriteLine("--> Retrieved data from cache.");
                return cachedPlatforms;
            }

            var platforms = await _context.Platforms.ToListAsync();

            Console.WriteLine($"--> Retrieved data from database. Count: {platforms.Count}");

            _cacheService.SetData(cacheKey, platforms, TimeSpan.FromMinutes(cacheDurationInMinutes));

            return platforms;
        }




        public async Task<Models.Platfrom> GetPlafromById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            }

            var cacheKey = $"platform_{id}";
            var cachedPlatform = _cacheService.GetData<Models.Platfrom>(cacheKey);
            if (cachedPlatform != null)
            {
                return cachedPlatform;
            }

            var platform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
            if (platform != null)
            {
                _cacheService.SetData(cacheKey, platform, TimeSpan.FromMinutes(2));
                return platform;
            }
            else
            {
                _logger.LogInformation($"Platform with ID {id} was not found.");
            }
            return platform;


        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                return (await _context.SaveChangesAsync() >= 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes.");
                return false;
            }
        }

      
    }
}
