using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookSerialization
{
	[XmlType("catalog", Namespace = "http://library.by/catalog")]
	[XmlRoot(Namespace = "http://library.by/catalog", IsNullable = false)]
	public class Catalog
	{
		[XmlElement("book")]
		public List<Book> Books { get; set; }

		[XmlAttribute("date", DataType = "date")]
		public DateTime Date { get; set; }
	}
}
