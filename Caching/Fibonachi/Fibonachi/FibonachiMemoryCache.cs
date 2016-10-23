using System;
using System.Runtime.Caching;

namespace Fibonachi
{
	class FibonachiMemoryCache : ICache<FibonachiLine>
	{
		private readonly ObjectCache _cache = MemoryCache.Default;

		public FibonachiLine Get(FibonachiLine obj)
		{
			for (int i = obj.Length; i > 0; i--)
			{
				var result = _cache.Get(i.ToString()) as FibonachiLine;
				if(result == null) continue;
				result.Line.RemoveRange(result.Length, result.Line.Count - result.Length);
				return result;
			}
			return null;
		}

		public void Set(FibonachiLine obj)
		{
			_cache.Set(obj.Length.ToString(), obj, DateTimeOffset.Now.AddSeconds(10));
		}
	}
}
