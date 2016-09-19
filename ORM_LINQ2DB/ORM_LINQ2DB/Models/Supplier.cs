using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    [Table("Suppliers")]
    public class Supplier
    {
        [PrimaryKey]
        [Identity]
        [Column]
        public int SupplierID { get; set; }
        [Column]
        public string CompanyName { get; set; }
        [Column]
        public string ContactName { get; set; }
        [Column]
        public string ContactTitle { get; set; }
    }
}