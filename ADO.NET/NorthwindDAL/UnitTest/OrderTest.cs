using System;
using System.Collections.Generic;
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

            // все заказы
            var allOrders = orderRepository.GetOrders();
            Assert.IsTrue(allOrders.Count() > 0);
            
            // добавление заказа с деталями
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
            var order = orderRepository.GetOrderById((Int32)result, true);
            foreach (var orderDetail in order.OrderDetails)
                        {
                            Console.WriteLine("{0} {1} {2}", orderDetail.Product, orderDetail.Quantity, orderDetail.UnitPrice);
                        }

            //изменение заказа и проверка внесенных изменений
            order.OrderDetails[0].UnitPrice = 999999;
            order.OrderDetails[1].ProductId = 1;
            Assert.IsTrue(orderRepository.ChangeOrder(order));
            order = orderRepository.GetOrderById(order.OrderId, true);
            foreach (var orderDetail in order.OrderDetails)
            {
                Console.WriteLine("{0} {1} {2}", orderDetail.Product, orderDetail.Quantity, orderDetail.UnitPrice);
            }

            // проверка добавленного заказа
            Assert.IsTrue(order.OrderDetails.Count == 2);
            Assert.AreEqual(order.ShipName, "Ivan");
            Assert.AreEqual(order.Freight, 123.0M);

            //изменение статуса заказа
            var order1 = orderRepository.ProcessOrder(order, DateTime.Now);
            Assert.IsNotNull(order1);

            //нельзя отправить заказ, не приняв его в обработку
            Assert.IsNull(orderRepository.CompleteOrder(order, DateTime.Now));

            // нельзя изменить не новый заказ
            Assert.IsFalse(orderRepository.ChangeOrder(order1));

            // можно отправить только принятый в обработку заказ
            Assert.IsNotNull(orderRepository.CompleteOrder(order1, DateTime.Now));

            // удаление нового заказа
            Assert.IsTrue(orderRepository.DeleteOrder(order));

            //нельзя удалить и изменить несуществующий в БД заказ
            Assert.IsFalse(orderRepository.DeleteOrder(new Order()));
            Assert.IsFalse(orderRepository.ChangeOrder(new Order()));

            // тест хранимых процедур
            // CustOrderHist
            var statRep = new StatisticRepository(connectionString.ConnectionString, connectionString.ProviderName);
            var products = statRep.GetCustomersProducts("ERNSH");
            Assert.IsNotNull(products);
            foreach (var product in products)
            {
                Console.WriteLine("{0} - {1}", product.Product, product.Total);
            }

            // CustOrdersDetail
            var details = statRep.GetCustOrdersDetail(10248);
            Assert.IsNotNull(details);
            foreach (var detail in details)
            {
                Console.WriteLine("{0} - {1} - {2} - {3} - {4}", detail.Product, detail.UnitPrice, detail.Quantity, detail.Discount, detail.ExtendedPrice);
            }
        }
    }
}
