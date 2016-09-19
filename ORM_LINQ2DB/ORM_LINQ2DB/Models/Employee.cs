using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public IEnumerable<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
