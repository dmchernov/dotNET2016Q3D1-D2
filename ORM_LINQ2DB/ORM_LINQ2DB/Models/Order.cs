using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
	[Table("Orders")]
	public class Order
	{
		[PrimaryKey]
		[Identity]
		public int OrderID { get; set; }

		[Column]
		public int EmployeeID { get; set; }
		[Column]
		public DateTime? OrderDate { get; set; }

		[Column]
		public DateTime? ShippedDate { get; set; }

		[Column]
		public int ShipVia { get; set; }

		[Association(ThisKey = "EmployeeID", OtherKey = "EmployeeID")]
		public Employee Employee { get; set; }

		[Association(ThisKey = "ShipVia", OtherKey = "ShipperID")]
		public Shipper Shipper { get; set; }

		[Association(ThisKey = "OrderID", OtherKey = "OrderID")]
		public IList<OrderDetail> OrderDetails { get; set; }
	}
}
