using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindDAL.Models;

namespace NorthwindDAL.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Decimal AddOrder(Order order);
        Order GetOrderById(Int32 id, Boolean withDetails);
        Boolean DeleteOrder(Order order);
        Int32 ProcessOrder(Int32 id, DateTime orderDate);
        Int32 CompleteOrder(Int32 id, DateTime shippedDate);
    }
}
