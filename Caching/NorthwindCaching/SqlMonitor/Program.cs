using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CachingSolutionsSamples;
using NorthwindLibrary;

namespace SqlMonitor
{
	class Program
	{
		static bool isChanged = false;
		static void Main(string[] args)
		{
			SqlDependency.Start(@"data source =.; initial catalog = Northwind; integrated security = True;");

			using (var connection = new SqlConnection(@"data source =.; initial catalog = Northwind; integrated security = True;"))
			{
				connection.Open();
				using (var command = new SqlCommand("SELECT [CategoryID], [CategoryName], [Description] FROM [dbo].[Categories]", connection))
				{
					var dependency = new SqlDependency(command);
					dependency.OnChange += DependencyOnOnChange;

					for (int i = 1; i <= 10; i++)
					{
						using (var dbContext = new Northwind())
						{
							dbContext.Configuration.LazyLoadingEnabled = false;
							dbContext.Configuration.ProxyCreationEnabled = false;
							var cat = dbContext.Categories.Add(new Category() { CategoryName = "TestCategory" + i });
							dbContext.SaveChanges();
							Console.WriteLine($"Category {cat.CategoryName} was added");
							//Console.WriteLine($"Categories count: {dbContext.Categories.Count()}");
						}

						using (var reader = command.ExecuteReader())
						{
							if(reader.Read()) Console.WriteLine(reader.FieldCount);
						}
						Thread.Sleep(10000);
						if (isChanged) break;
					}
				}
				connection.Close();
			}

			SqlDependency.Stop(@"data source =.; initial catalog = Northwind; integrated security = True;");

			Console.WriteLine(isChanged);
			Console.ReadKey();
		}

		private static void DependencyOnOnChange(object sender, SqlNotificationEventArgs e)
		{
			Console.WriteLine("Changes detected");
			isChanged = true;
		}
	}
}
