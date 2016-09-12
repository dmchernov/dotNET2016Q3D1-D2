using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDAL.Interfaces;
using NorthwindDAL.Repositories;
using System.Configuration;
using System.Linq;
using NorthwindDAL.Enums;
using NorthwindDAL.Models;

namespace UnitTest
{
    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["UnitTests"];
            IOrderRepository orderRepository = new OrderRepository(connectionString.ConnectionString, connectionString.ProviderName);

            var allOrders = orderRepository.GetOrders();
            Assert.IsTrue(allOrders.Count() > 0);

            var result = orderRepository.AddOrder(new Order() {Freight = 123.0M, ShipName = "Ivan"});
            var order = orderRepository.GetOrderById((Int32)result, true);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(order.OrderDetails.Count == 0);
            Assert.AreEqual(order.ShipName, "Ivan");
            Assert.AreEqual(order.Freight, 123.0M);

            Assert.IsTrue(orderRepository.DeleteOrder(order));

            Assert.IsFalse(orderRepository.DeleteOrder(new Order()));
        }
    }
}
