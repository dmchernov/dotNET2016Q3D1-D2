using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDAL.Interfaces;
using NorthwindDAL.Repositories;
using System.Configuration;
using System.Linq;
using NorthwindDAL.Enums;
using NorthwindDAL.Models;
//using NUnit.Framework;
//using NUnit.Framework.Internal;

namespace UnitTest
{
	[TestClass]
	public class NorthwindDalTest
	{
		private ConnectionStringSettings connectionString;
		private IOrderRepository orderRepository;

		public NorthwindDalTest()
		{
			this.connectionString = ConfigurationManager.ConnectionStrings["UnitTests"];
			this.orderRepository = new OrderRepository(connectionString.ConnectionString, connectionString.ProviderName);
		}

		[TestMethod]
		public void GetAllOrdersTest()
		{
			var allOrders = orderRepository.GetOrders();
			Assert.IsTrue(allOrders.Count() > 0);

			var a = 0;
			foreach (var order in allOrders)
			{
				if (order.OrderDetails == null) continue;
				Console.WriteLine($"{order.OrderId} - {order.OrderDate} | {order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice):C}");
				if (a++ == 20) break;
			}
		}

		private Order AddOrderWithDetails()
		{
			Order orderWithDetails = new Order();
			orderWithDetails.Freight = 123.0M;
			orderWithDetails.ShipName = "Ivan";

			orderWithDetails.OrderDetails = new List<OrderDetails>();

			OrderDetails det = new OrderDetails();
			det.UnitPrice = 122;
			det.ProductId = 15;
			det.Quantity = 3;
			det.Discount = 0;
			orderWithDetails.OrderDetails.Add(det);

			OrderDetails det1 = new OrderDetails();
			det1.UnitPrice = 344;
			det1.ProductId = 10;
			det1.Quantity = 2;
			det1.Discount = 1;
			orderWithDetails.OrderDetails.Add(det1);

			var result = orderRepository.AddOrder(orderWithDetails);
			Assert.IsTrue(result > 0);

			// получение только что добавленного заказа с деталями
			var order1 = orderRepository.GetOrderById((Int32)result, true);
			foreach (var orderDetail in order1.OrderDetails)
			{
				Console.WriteLine("{0} {1} {2}", orderDetail.Product, orderDetail.Quantity, orderDetail.UnitPrice);
			}

			return order1;
		}

		[TestMethod]
		public void AddOrderTest()
		{
			var order = AddOrderWithDetails();

			// проверка добавленного заказа
			Assert.IsTrue(order.OrderDetails.Count == 2);
			Assert.AreEqual(order.ShipName, "Ivan");
			Assert.AreEqual(order.Freight, 123.0M);
		}

		[TestMethod]
		public void ChangeOrderDetailsTest()
		{
			//изменение заказа и проверка внесенных изменений
			var order1 = AddOrderWithDetails();

			order1.OrderDetails[0].UnitPrice = 999999;
			order1.OrderDetails[1].ProductId = 1;

			Assert.IsTrue(orderRepository.ChangeOrder(order1));
			order1 = orderRepository.GetOrderById(order1.OrderId, true);

			foreach (var orderDetail in order1.OrderDetails)
			{
				Console.WriteLine("{0} {1} {2}", orderDetail.Product, orderDetail.Quantity, orderDetail.UnitPrice);
			}

		}

		[TestMethod]
		public void ChangeOrderStatusTest()
		{
			var order1 = AddOrderWithDetails();
			//изменение статуса заказа
			var order2 = orderRepository.ProcessOrder(order1, DateTime.Now);
			Assert.IsNotNull(order1);

			//нельзя отправить заказ, не приняв его в обработку
			Assert.IsNull(orderRepository.CompleteOrder(order1, DateTime.Now));

			// нельзя изменить не новый заказ
			Assert.IsFalse(orderRepository.ChangeOrder(order2));

			// можно отправить только принятый в обработку заказ
			order2 = orderRepository.CompleteOrder(order2, DateTime.Now);
			Assert.IsNotNull(order2);

			// Нельзя изменить несуществующий в БД заказ
			Assert.IsFalse(orderRepository.ChangeOrder(new Order()));
		}

		[TestMethod]
		public void DeleteOrderTest()
		{
			var order = AddOrderWithDetails();
			// удаление нового заказа
			Assert.IsTrue(orderRepository.DeleteOrder(order));

			//нельзя удалить несуществующий в БД заказ
			Assert.IsFalse(orderRepository.DeleteOrder(new Order()));
		}

		[TestMethod]
		public void CustOrderHistTest()
		{
			var statRep = new StatisticRepository(connectionString.ConnectionString, connectionString.ProviderName);
			var products = statRep.GetCustomersProducts("ERNSH");
			Assert.IsNotNull(products);
			foreach (var product in products)
			{
				Console.WriteLine("{0} - {1}", product.Product, product.Total);
			}
		}

		[TestMethod]
		public void CustOrdersDetailTest()
		{
			var statRep = new StatisticRepository(connectionString.ConnectionString, connectionString.ProviderName);
			var details = statRep.GetCustOrdersDetail(10248);
			Assert.IsNotNull(details);
			foreach (var detail in details)
			{
				Console.WriteLine("{0} - {1:C} - {2} - {3} - {4:C}", detail.Product, detail.UnitPrice, detail.Quantity, detail.Discount, detail.ExtendedPrice);
			}
		}
	}
}
