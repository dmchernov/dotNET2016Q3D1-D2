using System.Collections.Generic;
using LinqToDB.Mapping;

namespace ORM_LINQ2DB.Models
{
    public class Region
    {
        [PrimaryKey]
        [Identity]
        public int RegionID { get; set; }
        
        [Column(CanBeNull = false)]
        public string RegionDescription { get; set; }

        [Association(ThisKey = "RegionID", OtherKey = "RegionID")]
        public IList<Territory> Territories { get; set; }
    }
}