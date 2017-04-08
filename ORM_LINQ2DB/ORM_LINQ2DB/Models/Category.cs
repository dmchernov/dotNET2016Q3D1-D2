using System.Collections.Generic;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey]
        [Identity]
        [Column]
        public int CategoryID { get; set; }
        [Column]
        public string CategoryName { get; set; }
        [Column]
        public string Description { get; set; }

        [Association(ThisKey = "CategoryID", OtherKey = "CategoryID")]
        public IList<Product> Products { get; set; }
    }
}