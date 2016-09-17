using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindDAL.Models;

namespace NorthwindDAL.Interfaces
{
    public interface IStatisticRepository
    {
        List<ProductsTotal> GetCustomersProducts(String customerId);
        List<OrderDetailsExtended> GetCustOrdersDetail(Int32 id);
    }
}
