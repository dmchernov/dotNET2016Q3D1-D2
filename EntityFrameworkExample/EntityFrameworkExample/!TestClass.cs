using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkExample.Model;
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

                Console.WriteLine(cat.CategoryName);

                foreach (var order in db.Orders)
                {
                    foreach (var orderDetail in order.Order_Details.Where(d => d.Product.Category == cat))
                    {
                        Console.WriteLine($"{orderDetail.Product.Category.CategoryName} -- {order.Customer.CompanyName} - {orderDetail.Product.ProductName} - {orderDetail.Quantity} - {orderDetail.UnitPrice} - {orderDetail.Discount}");
                    }
                }
            }
        }
    }
}
