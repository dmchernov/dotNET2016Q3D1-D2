using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
	public class MemoryCache<T> : ICache<T>
	{
		private CacheItemPolicy _policy;
		public MemoryCache() { }
		public MemoryCache (CacheItemPolicy policy)
		{
			_policy = policy;
			_policy.UpdateCallback += UpdateCallback;
		}

		private void UpdateCallback(CacheEntryUpdateArguments arguments)
		{
			cache.Remove(arguments.Key);
		}

		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_" + typeof(T);

		public IEnumerable<T> Get(string forUser)
		{
			return (IEnumerable<T>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<T> collection)
		{
			if (_policy == null)
				cache.Set(prefix + forUser, collection, new DateTimeOffset(DateTime.Now.AddSeconds(5)));
			else
			{
				cache.Set(prefix + forUser, collection, _policy);
			}
		}
	}
}
