using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    public class EmployeeTerritories
    {
        [Association(ThisKey = "EmployeeID", OtherKey = "EmployeeID")]
        public Employee Employee { get; set; }
        [Column]
        public int EmployeeID { get; set; }
        [Column]
        public string TerritoryID { get; set; }
        [Association(ThisKey = "TerritoryID", OtherKey = "TerritoryID")]
        public Territory Territory { get; set; }
    }
}
