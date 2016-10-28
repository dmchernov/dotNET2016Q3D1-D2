using System;
using System.Collections.Generic;
using NorthwindLibrary;
using StackExchange.Redis;
using System.IO;
using System.Runtime.Caching;
using System.Runtime.Serialization;

namespace CachingSolutionsSamples
{
	class RedisCache<T> : ICache<T>
	{
		private ConnectionMultiplexer redisConnection;
		string prefix = "Cache_" + typeof(T);
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<T>));

		public RedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public IEnumerable<T> Get(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<T>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set(string forUser, IEnumerable<T> collection, CacheItemPolicy policy = null)
		{
			var db = redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (collection == null)
			{
				db.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, collection);
				// Кэш действителен 5 секунд
				db.StringSet(key, stream.ToArray(), TimeSpan.FromSeconds(5));
			}
		}
	}
}
