﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_BL.Services.CachingService
{
    public class CachingService : ICachingService
    {

        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public CachingService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }

        public async Task<string> GetAsync(string key)
        {
            var redisValue = await _database.StringGetAsync(key);
            return redisValue.HasValue ? redisValue.ToString() : null;
        }

        public async Task SaveAsync(string key, string value)
        {
            await _database.StringSetAsync(key, value, TimeSpan.FromMilliseconds(500_000));
        }


    }
}
