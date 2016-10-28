using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Task.Serialization;

namespace Task
{
	[TestClass]
	public class SerializationSolutions
	{
		Northwind dbContext;

		[TestInitialize]
		public void Initialize()
		{
			dbContext = new Northwind();
		}

		[TestMethod]
		public void SerializationCallbacks()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(), true);
			var categories = dbContext.Categories.ToList();

			//var c = categories.First();

			var t = (dbContext as IObjectContextAdapter).ObjectContext;
			foreach (var category in categories)
			{
				t.LoadProperty(category, p => p.Products);
			}

			tester.SerializeAndDeserialize(categories);
		}

		[TestMethod]
		public void ISerializable() // Complete
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);
			var products = dbContext.Products.ToList();

			var t = (dbContext as IObjectContextAdapter).ObjectContext;
			foreach (var product in products)
			{
				t.LoadProperty(product, p => p.Category);
			}

			tester.SerializeAndDeserialize(products);
		}


		[TestMethod]
		public void ISerializationSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var surrogateSelector = new SurrogateSelector();
			surrogateSelector.AddSurrogate(typeof(Order_Detail), new StreamingContext(), new OrderDetailsSerializationSurrogate());

			var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(new NetDataContractSerializer() {SurrogateSelector = surrogateSelector}, true);
			var orderDetails = dbContext.Order_Details.ToList();

			var t = (dbContext as IObjectContextAdapter).ObjectContext;
			foreach (var od in orderDetails)
			{
				t.LoadProperty(od, p => p.Product);
				t.LoadProperty(od, p => p.Order);
			}

			tester.SerializeAndDeserialize(orderDetails);
		}

		[TestMethod]
		public void IDataContractSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = true;
			dbContext.Configuration.LazyLoadingEnabled = true;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(new DataContractSerializer(typeof(IEnumerable<Order>), new DataContractSerializerSettings() {DataContractSurrogate = new OrderDataContractSurrogate(), PreserveObjectReferences = true, DataContractResolver = new ProxyDataContractResolver() }), true);
			var orders = dbContext.Orders.ToList();

			var t = (dbContext as IObjectContextAdapter).ObjectContext;
			foreach (var order in orders)
			{
				t.LoadProperty(order, o => o.Shipper);
				t.LoadProperty(order, o => o.Customer);
				t.LoadProperty(order, o => o.Employee);
				t.LoadProperty(order, o => o.Order_Details);
			}

			tester.SerializeAndDeserialize(orders);
		}
	}
}
