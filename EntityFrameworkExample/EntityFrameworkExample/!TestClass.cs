using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkExample.Model;
using NUnit.Framework.Internal;

using NUnit.Framework;

namespace EntityFrameworkExample
{
	class TestClass
	{
		[Test]
		public void TestMethod()
		{
			using (var db = new Northwind())
			{
				var cat = db.Categories.First();

				foreach (var order in db.Orders)
				{
					foreach (var orderDetail in order.Order_Details.Where(d => d.Product.Category == cat))
					{
						Console.WriteLine($"Category: {orderDetail.Product.Category.CategoryName}\nCustomer: {order.Customer.CompanyName}\nDetails:\n\tProduct: {orderDetail.Product.ProductName}\n\tQuantity: {orderDetail.Quantity}\n\tUnitPrice: {orderDetail.UnitPrice:C}\n\tDiscount: {orderDetail.Discount:P}");
					}
				}
			}
		}
	}
}
