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
        private MemoryCache memoryCache;
        public ResultsStorage()
        {
            memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
        
        public FinalMessage this[string id] {
            set
            {
                memoryCache.Set(id, value, TimeSpan.FromMinutes(1));
            }
        }

        public bool TryRemove(string id, out FinalMessage result)
        {
            if (!memoryCache.TryGetValue(id, out result))
            {
                return false;
            }

            memoryCache.Remove(id);
            return true;
        }
    }
}