using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    [Table("Products")]
    public class Product
    {
        [PrimaryKey]
        [Identity]
        [Column]
        public int ProductID { get; set; }
        [Column]
        public string ProductName { get; set; }
        [Column]
        public int SupplierID { get; set; }
        [Column]
        public int CategoryID { get; set; }
        [Column]
        public string QuantityPerUnit { get; set; }
        [Column]
        public decimal UnitPrice { get; set; }
        [Column]
        public short UnitsInStock { get; set; }
        [Column]
        public short UnitsOnOrder { get; set; }
        [Column]
        public short ReorderLevel { get; set; }
        [Column]
        public bool Discontinued { get; set; }

        [Association(ThisKey = "SupplierID", OtherKey = "SupplierID")]
        public Supplier Supplier { get; set; }

        [Association(ThisKey = "CategoryID", OtherKey = "CategoryID")]
        public Category Category { get; set; }

    }
}
