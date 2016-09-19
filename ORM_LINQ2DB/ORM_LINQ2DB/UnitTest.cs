using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
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
                //LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
                //var cat = connection.GetTable<Category>().LoadWith(c => c.Products).ToList()[0];
                //foreach (var prod in cat.Products)
                //{
                //    Console.WriteLine($"{prod.ProductName}");
                //}
            }
        }
    }
}
