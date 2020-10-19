using System;
using Microsoft.Extensions.Caching.Memory;
using SendColorBot.Models;

namespace SendColorBot
{
    /// <summary>
    /// Stores Inline Query results to replace preview with final photo
    /// </summary>
    public class ResultsStorage
    {
        private readonly MemoryCache _memoryCache;
        private readonly TimeSpan _inlineQueryLifetime = TimeSpan.FromMinutes(1);
        
        public ResultsStorage()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
        
        public FinalMessage this[string id]
        {
            set
            {
                _memoryCache.Set(id, value, _inlineQueryLifetime);
            }
        }

        public bool TryRemove(string id, out FinalMessage result)
        {
            if (!_memoryCache.TryGetValue(id, out result))
                return false;

            _memoryCache.Remove(id);
            return true;
        }
    }
}