using System.Collections.Generic;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    [Table("Territories")]
    public class Territory
    {
        [PrimaryKey]
        [Identity]
        [Column]
        public int TerritoryID { get; set; }
        [Column]
        public string TerritoryDescription { get; set; }

        [Association(ThisKey = "TerritoryID", OtherKey = "TerritoryID")]
        public IList<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}