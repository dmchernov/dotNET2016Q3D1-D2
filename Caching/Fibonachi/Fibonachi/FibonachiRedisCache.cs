using System;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace Fibonachi
{
	class FibonachiRedisCache: ICache<FibonachiLine>
	{
		private ConnectionMultiplexer redisConnection;
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(FibonachiLine));

		public FibonachiRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
			var db = redisConnection.GetDatabase();
		}

		public FibonachiLine Get(FibonachiLine obj)
		{
			var db = redisConnection.GetDatabase();
			for (int i = obj.Length; i > 0; i--)
			{
				byte[] s = db.StringGet(i.ToString());
				if (s == null) continue;

				return (FibonachiLine)serializer
					.ReadObject(new MemoryStream(s));
			}

			return null;
		}

		public void Set(FibonachiLine obj)
		{
			var db = redisConnection.GetDatabase();
			var key = obj?.Length;

			if (obj == null)
			{
				db.StringSet(0.ToString(), RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, obj);
				db.StringSet(key.ToString(), stream.ToArray(), TimeSpan.FromSeconds(120));
			}
		}
	}

}
