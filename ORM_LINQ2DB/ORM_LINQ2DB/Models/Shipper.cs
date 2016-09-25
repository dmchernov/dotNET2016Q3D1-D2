using System;
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
	[Table("Shippers")]
	public class Shipper
	{
		[PrimaryKey]
		[Identity]
		public int ShipperID { get; set; }

		[Column]
		public String CompanyName { get; set; }

		[Association(ThisKey = "ShipperID", OtherKey = "ShipVia")]
		private IList<Order> Orders { get; set; }
	}
}