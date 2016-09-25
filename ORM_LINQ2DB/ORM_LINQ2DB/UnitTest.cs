using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.Informix;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORM_LINQ2DB.Models;

namespace ORM_LINQ2DB
{
	[TestClass]
	public class UnitTest
	{
		[TestMethod]
		public void ProductWithCategoryAndSupplier()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				foreach (var prod in connection.GetTable<Product>().LoadWith(p => p.Category).LoadWith(p => p.Supplier))
					Console.WriteLine("{0} - {1} | {2}", prod.ProductName, prod.Category.CategoryName, prod.Supplier.CompanyName);
			}
		}

		[TestMethod]
		public void EmployeesWithTerritories()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				var result = from emp in connection.GetTable<Employee>().ToList()
							 join empTer in connection.GetTable<EmployeeTerritories>() on emp.EmployeeID equals empTer.EmployeeID
							 join ter in connection.GetTable<Territory>() on empTer.TerritoryID equals ter.TerritoryID
					select new {Emp = emp, Ter = ter};
							 

				foreach (var emp in result)
					Console.WriteLine("{0} {1} - {2}", emp.Emp.FirstName, emp.Emp.LastName, emp.Ter.TerritoryDescription);
			}
		}

		[TestMethod]
		public void CountEmployeeForRegion()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				var result = from emp in connection.GetTable<Employee>()
							 join empTer in connection.GetTable<EmployeeTerritories>() on emp.EmployeeID equals empTer.EmployeeID
							 join ter in connection.GetTable<Territory>() on empTer.TerritoryID equals ter.TerritoryID
							 join reg in connection.GetTable<Region>() on ter.RegionID equals reg.RegionID
							 select new { Reg = reg, Emp = emp };

				var groupRes = from r in result
							   group r by r.Reg
					into reg
							   select new
							   {
								   Reg = reg.Key,
								   Count = result.Distinct().Count(r => r.Reg == reg.Key)
							   };

				foreach (var res in groupRes)
				{
					Console.WriteLine("{0} - {1}", res.Reg.RegionDescription, res.Count);
				}
			}
		}

		[TestMethod]
		public void EmployeesWithShippers()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

				var result = from emp in connection.GetTable<Employee>()
							 join order in connection.GetTable<Order>() on emp.EmployeeID equals order.EmployeeID
							 join shipper in connection.GetTable<Shipper>() on order.ShipVia equals shipper.ShipperID
							 select new { Emp = emp, Ship = shipper };

				var groupResult = from r in result
					group r by r.Emp
					into groupEmp
					select new
					{
						Emp = groupEmp.Key,
						Ship = result.Distinct().Select(s => s.Ship)
					};

				foreach (var res in groupResult)
				{
					foreach (var shipper in res.Ship.Distinct())
					{
						Console.WriteLine($"{res.Emp.FirstName} {res.Emp.LastName} - {shipper.CompanyName}");
					}
				}
			}
		}

		[TestMethod]
		public void AddEmployee()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				var employee = new Employee() {FirstName = "Ivan", LastName = "Ivanov", Title = "Manager"};
				var insertedEmpId = (decimal)connection.InsertWithIdentity(employee);
				var insertedEmployee = connection.GetTable<Employee>().Where(e => e.EmployeeID == (int)insertedEmpId).FirstOrDefault();
				Assert.IsNotNull(insertedEmployee);
				var empTer1 = new EmployeeTerritories() {EmployeeID = insertedEmployee.EmployeeID, TerritoryID = connection.GetTable<Territory>().OrderByDescending(t => t.TerritoryDescription).First().TerritoryID};
				var empTer2 = new EmployeeTerritories() {EmployeeID = insertedEmployee.EmployeeID, TerritoryID = connection.GetTable<Territory>().OrderBy(t => t.TerritoryDescription).First().TerritoryID};
				insertedEmployee.EmployeeTerritories = new List<EmployeeTerritories>();
				insertedEmployee.EmployeeTerritories.Add(empTer1);
				insertedEmployee.EmployeeTerritories.Add(empTer2);

				foreach (var employeeTerritory in insertedEmployee.EmployeeTerritories)
				{
					var a = connection.InsertWithIdentity(employeeTerritory);
					Assert.IsNotNull(a);
				}

				Console.WriteLine($"{insertedEmployee.EmployeeID} - {insertedEmployee.FirstName} {insertedEmployee.LastName}");

				var result = from emp in connection.GetTable<Employee>().ToList()
							 join empTer in connection.GetTable<EmployeeTerritories>() on emp.EmployeeID equals empTer.EmployeeID
							 join ter in connection.GetTable<Territory>() on empTer.TerritoryID equals ter.TerritoryID
							 where emp.EmployeeID == (int)insertedEmpId
							 select new { Emp = emp, Ter = ter };

				foreach (var emp in result)
					Console.WriteLine("{0} {1} - {2}", emp.Emp.FirstName, emp.Emp.LastName, emp.Ter.TerritoryDescription);

			}
		}

		[TestMethod]
		public void ChangeCategory()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				var products = connection.GetTable<Product>().LoadWith(p => p.Category).Where(p => p.ProductName.StartsWith("C")).ToList();
				var categories = connection.GetTable<Category>().ToList();
				foreach (var product in products)
				{
					Console.Write($"Old cat: {product.Category.CategoryName}; ");
					var cat = categories.First(c => c.CategoryID != product.CategoryID);
					product.CategoryID = cat.CategoryID;
					Assert.AreEqual(connection.Update(product), 1);
					Console.WriteLine($"new cat: {connection.GetTable<Product>().LoadWith(p => p.Category).First(p => p.ProductID == product.ProductID).Category.CategoryName}");
				}
			}
		}

		[TestMethod]
		public void AddProducts()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				var oldSupplier = connection.GetTable<Supplier>().FirstOrDefault();
				Assert.IsNotNull(oldSupplier);
				var newSuppler = new Supplier() {CompanyName = "Microsoft", ContactName = "Bill Gaits"};

				var oldCategory = connection.GetTable<Category>().FirstOrDefault();
				Assert.IsNotNull(oldCategory);
				var newCategory = new Category() {CategoryName = "Software"};

				var myProducts = new List<Product>()
				{
					new Product() {Category = oldCategory, Supplier = oldSupplier, ProductName = "TestProd"},
					new Product() {Category = newCategory, Supplier = newSuppler, ProductName = "X-box One"},
					new Product() {Category = newCategory, Supplier = newSuppler, ProductName = "Lumia 950"},
				};

				foreach (var product in myProducts)
				{
					// Category
					int catForInsert;
					var existingCat =
						connection.GetTable<Category>().FirstOrDefault(c => c.CategoryName == product.Category.CategoryName);
					catForInsert = (existingCat == null) ? (int) (decimal) connection.InsertWithIdentity(product.Category) : existingCat.CategoryID;
					
					// Supplier
					int suppForInsert;
					var existingSupplier = connection.GetTable<Supplier>().FirstOrDefault(s => s.CompanyName == product.Supplier.CompanyName);
					suppForInsert = (existingSupplier == null) ? (int) (decimal) connection.InsertWithIdentity(product.Supplier) : existingSupplier.SupplierID;
					
					// Product
					product.CategoryID = catForInsert;
					product.SupplierID = suppForInsert;
					var insertedProductId = (int)(decimal)connection.InsertWithIdentity(product);

					// Проверка вставки
					var insertedProduct = from p in connection.GetTable<Product>()
						join c in connection.GetTable<Category>() on p.CategoryID equals c.CategoryID
						join supplier in connection.GetTable<Supplier>() on p.SupplierID equals supplier.SupplierID
						where p.ProductID == insertedProductId
						select new {Prod = p, Cat = c, Supp = supplier};

					foreach (var prod in insertedProduct)
					{
						Console.WriteLine($"({prod.Prod.ProductID}) Name: {prod.Prod.ProductName}, Category: ({prod.Cat.CategoryID}) {prod.Cat.CategoryName}, Supplier: ({prod.Supp.SupplierID}) {prod.Supp.ContactName}");
					}
				}
			}
		}

		[TestMethod]
		public void ChangeOrders()
		{
			using (var connection = new DataConnection("Northwind"))
			{
				LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

				var orders = connection.GetTable<Order>().LoadWith(o => o.OrderDetails).Where(o => o.ShippedDate == null).ToList();
				Assert.IsNotNull(orders);
				Assert.IsTrue(orders.Count > 0);

				var products = connection.GetTable<Product>().ToList();
				
				foreach (var order in orders)
				{
					foreach (var orderDetail in order.OrderDetails)
					{
						// Получаем текущий продукт
						var currentProduct = products.FirstOrDefault(p => p.ProductID == orderDetail.ProductID);
						Assert.IsNotNull(currentProduct);

						Console.WriteLine($"Order № {order.OrderID}:\nCurrent product: ({currentProduct.ProductID}) {currentProduct.ProductName} (Category {currentProduct.CategoryID}); ");

						// Новый продукт из этой же категории
						Product newProduct = products.FirstOrDefault(p => p.ProductID != orderDetail.ProductID && p.CategoryID == currentProduct.CategoryID);
						Assert.IsNotNull(newProduct);

						var actualOrderDet = connection.GetTable<OrderDetail>().Where(od => od.OrderID == orderDetail.OrderID).ToList();
						foreach (var product in products.Where(p => p.CategoryID == currentProduct.CategoryID))
						{
							if (!actualOrderDet.Select(id => id.ProductID).Contains(product.ProductID))
							{
								newProduct = products.FirstOrDefault(p => p.ProductID == product.ProductID);
								Assert.IsNotNull(newProduct);
								break;
							}
						}
						// Обновление текущей записи в Order Details
						connection.GetTable<OrderDetail>()
							.Where(od => od.ProductID == orderDetail.ProductID && od.OrderID == orderDetail.OrderID).Set(od => od.ProductID, od => newProduct.ProductID).Update();

						var orDet =
							connection.GetTable<OrderDetail>()
								.FirstOrDefault(
									od =>
										od.OrderID == orderDetail.OrderID && od.ProductID != orderDetail.ProductID &&
										od.ProductID == newProduct.ProductID);
						Assert.IsNotNull(orDet);

						// Получение обновленной записи и вывод в консоль
						Console.WriteLine($"New product: ({newProduct.ProductID}) {newProduct.ProductName} (Category {newProduct.CategoryID})\n\n");
					}
				}
			}
		}
	}
}
