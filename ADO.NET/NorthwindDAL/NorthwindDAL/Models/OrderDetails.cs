using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL.Models
{
    public class OrderDetails
    {
        public Int32 ProductId { get; set; }
        public String Product { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Discount { get; set; }
    }
}
