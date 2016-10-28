using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
	public interface ICache<T>
	{
		IEnumerable<T> Get(string forUser);
		void Set(string forUser, IEnumerable<T> collection, CacheItemPolicy policy = null);
	}
}
