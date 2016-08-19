// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void Linq1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 1")]
		[Description("Список всех клиентов, чей суммарный оборот превышает Х")]

		public void Linq001()
		{
			var totals = new Decimal[] {30000.0M, 40000.0M, 100000.0M};

			foreach (var t in totals)
			{
				var clients = dataSource.Customers.Where(c => c.Orders.Sum(o => o.Total) > t);

				Console.WriteLine($"Суммарный оборот превышает {t}:");

				foreach (var c in clients)
				{
					ObjectDumper.Write(c.CustomerID);
				}
				
				Console.WriteLine($"\nВсего клиентов: {clients.Count()}");
				Console.WriteLine(@"-------------------------------------");
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 2")]
		[Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без.")]

		public void Linq002()
		{
			var resultWithoutGroup = from cust in dataSource.Customers
						 orderby cust.City
						 select new
						 {
						 	cust.Country,
						 	cust.City,
						 	cust.CustomerID,
						 	suppliers = from s in dataSource.Suppliers
						 				where s.Country == cust.Country && s.City == cust.City
						 				select s.SupplierName
						 };

			var resultWithGroup = from cust in dataSource.Customers
				group cust by cust.Country
				into country
				select new
				{
					Country = country.Key,
					City = from city in country
						   group city by city.City into city
						   select new
						   {
							   City = city.Key,
							   Customers = city,
							   Suppliers = dataSource.Suppliers.Where(s => s.Country == country.Key && s.City == city.Key)
						   }
				};

			Console.WriteLine("Результаты без группировки:");
			Console.WriteLine("{0, -10}{1, -10}{2, -10}{3}", "Country", "City", "Customer", "Supplier");
			foreach (var r in resultWithoutGroup)
			{
				foreach (var s in r.suppliers)
				{
					Console.WriteLine("{0, -10}{1, -10}{2, -30}", r.Country, r.City, r.CustomerID, s);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Результаты с группировкой:");
			Console.WriteLine("{0, -10}{1, -10}{2, -10}{3}", "Country", "City", "Customer", "Supplier");
			foreach (var country in resultWithGroup)
			{
				foreach (var city in country.City)
				{
					foreach (var customer in city.Customers)
					{
						foreach (var supplier in city.Suppliers)
						{
							Console.WriteLine("{0, -10}{1, -10}{2, -10}{3}", country.Country, city.City, customer.CustomerID, supplier.SupplierName);
						}
					}
				}
				
				
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 3")]
		[Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]

		public void Linq003()
		{
			var amounts = new Decimal[] { 6000.0M, 8000.0M, 10000.0M };

			foreach (var a in amounts)
			{
				var clients = dataSource.Customers.Where(c => c.Orders.Any(o => o.Total > a));

				Console.WriteLine($"Есть заказы, превышающие {a}:");

				foreach (var c in clients)
				{
					ObjectDumper.Write(c.CustomerID);
				}

				Console.WriteLine($"Всего клиентов: {clients.Count()}");
				Console.WriteLine(@"-------------------------------------");
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 4")]
		[Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]

		public void Linq004()
		{
			var clients =
				dataSource.Customers.Where(c => c.Orders.Length > 0).Select(
					c =>
						new
						{
							Name = c.CustomerID,
							Date = c.Orders.Min(o => o.OrderDate)
						});

			Console.WriteLine("{0,-10}{1,-15}{2,-20}","Client", "Year", "Month");
			foreach (var c in clients)
			{
				Console.WriteLine("{0,-10}{1,-15}{2,-20}", c.Name, c.Date.Year, c.Date.Month);
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 5")]
		[Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]

		public void Linq005()
		{
			var clients =
				dataSource.Customers.Where(c => c.Orders.Length > 0).Select(
					c =>
						new
						{
							Cust = c,
							Year = c.Orders.Min(o => o.OrderDate).Year,
							Month = c.Orders.Min(o => o.OrderDate).Month,
							Turn = c.Orders.Sum(o => o.Total)
						}).OrderBy(c => c.Year).ThenBy(c => c.Month).ThenByDescending(c => c.Turn).ThenBy(c => c.Cust.CustomerID);

			Console.WriteLine("{0,-10}{1,-15}{2,-20}{3,-25}", "Year", "Month", "Turn", "Client");
			foreach (var c in clients)
			{
				Console.WriteLine("{0,-10}{1,-15}{2,-20}{3,-25}", c.Year, c.Month, c.Turn, c.Cust.CustomerID);
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 6")]
		[Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион" +
					 " или в телефоне не указан код оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).")]

		public void Linq006()
		{
			var clients =
				dataSource.Customers.Where(c => !c.PostalCode.IsNumber() || String.IsNullOrEmpty(c.Region) || c.Phone[0] != '(');

			Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-25}", "PostIndex", "Region", "Phone", "Client");
			foreach (var c in clients)
			{
				Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-25}", c.PostalCode, c.Region, c.Phone, c.CustomerID);
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 7")]
		[Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]

		public void Linq007()
		{
			var products =	from product in dataSource.Products
							group product by product.Category
							into prodCat
							orderby prodCat.Key
							select new
							{
								Category = prodCat.Key,
								IsStockGroups = from p in prodCat
								group p by (p.UnitsInStock > 0)
								into unitInStock
								select new {IsStock = unitInStock.Key, Products = from product in unitInStock orderby product.UnitPrice select product}
							};

			Console.WriteLine("{0,-20}{1,-10}{2,-40}{3}", "Category", "InStock", "ProductName", "Price");
			Console.WriteLine(@"--------------------------------------------------------------------------------");
			foreach (var prodGroup in products)
				{
					foreach (var prodInStockGroup in prodGroup.IsStockGroups)
					{
						foreach (var product in prodInStockGroup.Products)
						{
							Console.WriteLine("{0,-20}{1,-10}{2,-40}{3}", prodGroup.Category, prodInStockGroup.IsStock, product.ProductName, product.UnitPrice);
						}
					}
					Console.WriteLine(@"--------------------------------------------------------------------------------");
				}
		}

		[Category("LINQ Tasks")]
		[Title("Task 8")]
		[Description("Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами")]

		public void Linq008()
		{
			var products = from product in dataSource.Products
				group product by
					new
					{
						low = product.UnitPrice < 30,
						middle = product.UnitPrice >= 30 && product.UnitPrice < 50,
						high = product.UnitPrice >= 50
					}
				into cost
				select new
				{
					Cost = cost.Key,
					Products = from p in cost orderby p.UnitPrice select p
				};

			Console.WriteLine("{0,-20}{1,-10}{2,-40}{3}", "Category", "InStock", "ProductName", "Price");
			Console.WriteLine(@"--------------------------------------------------------------------------------");
			foreach (var prod in products)
			{
				foreach (var p in prod.Products)
				{
					Console.WriteLine("{0,-20}{1,-10}{2,-40}{3}", prod.Cost.high ? "Дорого" : prod.Cost.middle ? "Средне" : "Дешево", p.UnitsInStock > 0, p.ProductName, p.UnitPrice);
				}
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 9")]
		[Description("Рассчитайте среднюю прибыльность каждого города (среднюю сумму заказа по всем клиентам из данного города)" +
					" и среднюю интенсивность (среднее количество заказов, приходящееся на клиента из каждого города)")]
		public void Linq009()
		{
			var citysAvg = from customer in dataSource.Customers
				group customer by customer.City
				into city
				orderby city.Key
				select new
				{
					City = city.Key,
					AVG = city.Where(c => c.Orders.Length > 0).Average(c => c.Orders.Average(o => o.Total))
				};

			Console.WriteLine("{0,-20}{1}", "City", "Средняя прибыльность");
			Console.WriteLine(@"--------------------------------------------------------------------------------");

			foreach (var сi in citysAvg)
			{
				Console.WriteLine("{0,-20}{1:C}", сi.City, сi.AVG);
			}

			Console.WriteLine();

			var citysCount = from customer in dataSource.Customers
				group customer by customer.City
				into city
				orderby city.Key
				select new
				{
					City = city.Key,
					Count = city.Average(c => c.Orders.Length)
				};

			Console.WriteLine("{0,-20}{1}", "City", "Среднее число заказов на одного клиента");
			Console.WriteLine(@"--------------------------------------------------------------------------------");

			foreach (var сi in citysCount)
			{
				Console.WriteLine("{0,-20}{1:F1}", сi.City, сi.Count);
			}
		}

		[Category("LINQ Tasks")]
		[Title("Task 10")]
		[Description("Сделайте среднегодовую статистику активности клиентов по месяцам (без учета года), статистику по годам, по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение).")]

		public void Linq010()
		{
			var activeMonth = from customer in dataSource.Customers
										 orderby customer.CustomerID
				select new
				{
					Customer = customer,
					ActiveMonth = from order in customer.Orders
								  group order by order.OrderDate.Month into month
								  orderby month.Key
								  select new
								  {
									  Month = month.Key,
									  Count = month.Count()
								  }
				};

			var activeYear = from customer in dataSource.Customers
										 orderby customer.CustomerID
				select new
				{
					Customer = customer,
					ActiveYear = from order in customer.Orders
								  group order by order.OrderDate.Year into year
								  orderby year.Key
								  select new
								  {
									  Year = year.Key,
									  Count = year.Count()
								  }
				};

			var activeYearMonth = from customer in dataSource.Customers
										 orderby customer.CustomerID
				select new
				{
					Customer = customer,
					ActiveYear = from order in customer.Orders
								  group order by order.OrderDate.Year into year
								  orderby year.Key
								  select new
								  {
									  Year = year.Key,
									  Month = from y in year
											  group y by y.OrderDate.Month into month
											  select new
											  {
												  Month = month.Key,
												  Count = month.Count()
											  }
								  }
				};

			Console.WriteLine("Помесячно");
			Console.WriteLine("{0,-20}{1,-10}{2}", "Customer", "Month", "Count");
			Console.WriteLine(@"--------------------------------------------------------------------------------");
			foreach (var item in activeMonth)
			{
				foreach (var p in item.ActiveMonth)
				{
					Console.WriteLine("{0,-20}{1,-10}{2}", item.Customer.CustomerID, p.Month, p.Count);
				}
			}
			Console.WriteLine();

			Console.WriteLine("По годам");
			Console.WriteLine("{0,-20}{1,-10}{2}", "Customer", "Year", "Count");
			Console.WriteLine(@"--------------------------------------------------------------------------------");
			foreach (var item in activeYear)
			{
				foreach (var p in item.ActiveYear)
				{
					Console.WriteLine("{0,-20}{1,-10}{2}", item.Customer.CustomerID, p.Year, p.Count);
				}
			}
			Console.WriteLine();

			Console.WriteLine("Помесячно и по годам");
			Console.WriteLine("{0,-10}{1,-10}{2,-10}{3}", "Customer", "Year", "Month", "Count");
			Console.WriteLine(@"--------------------------------------------------------------------------------");
			foreach (var ym in activeYearMonth)
			{
				foreach (var y in ym.ActiveYear)
				{
					foreach (var m in y.Month)
					{
					Console.WriteLine("{0,-10}{1,-10}{2,-10}{3}", ym.Customer.CustomerID, y.Year, m.Month, m.Count);
					}
				}
			}
		}
	}
}
