using PlatformService.Data.Repos.Caching;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace BlogApp.Net.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cachedb;

        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _cachedb = connectionMultiplexer.GetDatabase();
        }

        public  T GetData<T>(string key)
        {
            try
            {
                var value = _cachedb.StringGet(key);
                if (!value.IsNullOrEmpty)
                {
                    return value.HasValue? JsonSerializer.Deserialize<T>(value):default;
                }
                return default;
            }
            catch (RedisConnectionException ex)
            {
                // Handle the exception or log it
                throw new Exception("Error retrieving data from cache.", ex);
            }
        }

        public object RemoveData(string key)
        {
            try
            {
                var exists = _cachedb.KeyExists(key);
                if (exists)
                {
                    return _cachedb.KeyDelete(key);
                }
                return false;
            }
            catch (RedisConnectionException ex)
            {
                // Handle the exception or log it
                throw new Exception("Error removing data from cache.", ex);
            }
        }

        public bool SetData<T>(string key, T value, TimeSpan expirationTime)
        {
            try
            {
                var adjustedExpiration = expirationTime.Add(TimeSpan.FromMinutes(2));
                return _cachedb.StringSet(key, JsonSerializer.Serialize(value), adjustedExpiration);
            }
            catch (RedisConnectionException ex)
            {
                // Handle the exception or log it
                throw new Exception("Error setting data in cache.", ex);
            }
        }

        public bool UpdateCacheIfExists<T>(string key, T value, TimeSpan expirationTime)
        {
            try
            {
                var exists = _cachedb.KeyExists(key);
                if (exists)
                {
                    var serializedValue = JsonSerializer.Serialize(value);
                    return _cachedb.StringSet(key, serializedValue, expirationTime);
                }
                return false;
            }
            catch (RedisConnectionException ex)
            {
                // Handle the exception or log it
                throw new Exception("Error updating data in cache.", ex);
            }
        }
    }
}
