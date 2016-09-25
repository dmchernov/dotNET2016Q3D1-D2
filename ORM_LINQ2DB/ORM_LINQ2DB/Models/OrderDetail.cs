using System;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
	[Table("Order Details")]
	public class OrderDetail
	{
		[PrimaryKey]
		public int OrderID { get; set; }

		[PrimaryKey]
		public int ProductID { get; set; }

		[Column]
		public decimal UnitPrice { get; set; }

		[Column]
		public short Quantity { get; set; }

		[Column]
		public decimal Discount { get; set; }

		[Association(ThisKey = "OrderID", OtherKey = "OrderID")]
		public Order Order { get; set; }

		[Association(ThisKey = "ProducID", OtherKey = "ProductID")]
		public Product Product { get; set; }
	}
}