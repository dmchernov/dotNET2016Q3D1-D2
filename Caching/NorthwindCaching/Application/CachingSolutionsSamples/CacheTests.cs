﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Data.SqlClient;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestMethod]
		public void CategoriesMemoryCacheTest()
		{
			var manager = new EntityManager<Category>(new MemoryCache<Category>());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.Get().Count());
				Thread.Sleep(1000);
			}
		}

		[TestMethod]
		public void ProductsMemoryCacheTest()
		{
			var manager = new EntityManager<Product>(new MemoryCache<Product>());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.Get().Count());
				Thread.Sleep(1000);
			}
		}

		[TestMethod]
		public void CustomersMemoryCacheTest()
		{
			var manager = new EntityManager<Customer>(new MemoryCache<Customer>());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.Get().FirstOrDefault()?.CompanyName);
				Thread.Sleep(1000);
			}
		}

		[TestMethod]
		public void CategoriesRedisCacheTest()
		{
			var manager = new EntityManager<Category>(new RedisCache<Category>("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.Get().Count());
				Thread.Sleep(1000);
			}
		}

		[TestMethod]
		public void ProductsRedisCacheTest()
		{
			var manager = new EntityManager<Product>(new RedisCache<Product>("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.Get().Count());
				Thread.Sleep(1000);
			}
		}

		[TestMethod]
		public void CustomersRedisCacheTest()
		{
			var manager = new EntityManager<Customer>(new RedisCache<Customer>("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.Get().FirstOrDefault()?.CompanyName);
				Thread.Sleep(1000);
			}
		}

		[TestMethod]
		public void SereverMonitorCacheTest()
		{
			// Мониторинг БД не работает
			var policy = new CacheItemPolicy();
			policy.ChangeMonitors.Add(new SqlChangeMonitor(new SqlDependency(new SqlCommand("SELECT [CategoryID],[CategoryName],[Description],[Picture] FROM [Northwind].[dbo].[Categories]"))));

			var manager = new EntityManager<Category>(new MemoryCache<Category>(policy));

			for (int i = 0; i < 10; i++)
			{
				var categories = manager.Get();
				foreach (var category in categories)
				{
					Console.WriteLine(category.CategoryName);
				}

				if (i == 3 || i == 6)
				{
					using (var dbContext = new Northwind())
					{
						dbContext.Configuration.LazyLoadingEnabled = false;
						dbContext.Configuration.ProxyCreationEnabled = false;
						dbContext.Categories.Add(new Category() {CategoryName = "TestCategory" + i});
						dbContext.SaveChanges();
					}
				}
				Thread.Sleep(10000);
			}
		}
	}
}