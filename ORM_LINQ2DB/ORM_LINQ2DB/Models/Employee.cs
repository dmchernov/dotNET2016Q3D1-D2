using System.Collections.Generic;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    [Table("Employees")]
    public class Employee
    {
        [PrimaryKey]
        [Identity]
        [Column]
        public int EmployeeID { get; set; }
        [Column]
        public string LastName { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public string Title { get; set; }
        [Association(ThisKey = "EmployeeID", OtherKey = "EmployeeID")]
        public IList<EmployeeTerritories> EmployeeTerritories { get; set; }

		[Association(ThisKey = "EmployeeID", OtherKey = "EmployeeID")]
		public IList<Order> Orders { get; set; }
    }
}
