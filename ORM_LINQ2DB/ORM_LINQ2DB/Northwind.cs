using LinqToDB;
using LinqToDB.Data;
using ORM_LINQ2DB.Models;

namespace ORM_LINQ2DB
{
    public class Northwind : DataConnection
    {
        public Northwind() : base("Northwind")
        { }

        public ITable<Product> Products { get { return GetTable<Product>(); } }
        public ITable<Employee> Employees { get { return GetTable<Employee>(); } }
        public ITable<EmployeeTerritories> EmployeeTerritories { get { return GetTable<EmployeeTerritories>(); } }
    }
}
