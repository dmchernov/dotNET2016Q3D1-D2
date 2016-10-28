using System.IO;
using System.Runtime.Serialization;

namespace Task.TestHelpers
{
	public class XmlDataContractSerializerTester<T> : SerializationTester<T, XmlObjectSerializer>
	{
		public XmlDataContractSerializerTester(
			XmlObjectSerializer serializer, bool showResult = false) : base(serializer, showResult)
		{ }

		internal override T Deserialization(MemoryStream stream)
		{
			return (T)serializer.ReadObject(stream);
		}

		internal override void Serialization(T data, MemoryStream stream)
		{
			serializer.WriteObject(stream, data);
		}
	}
}
