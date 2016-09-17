using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL.Models
{
    public class OrderDetailsExtended : OrderDetails
    {
        public Decimal ExtendedPrice { get; set; }
    }
}
