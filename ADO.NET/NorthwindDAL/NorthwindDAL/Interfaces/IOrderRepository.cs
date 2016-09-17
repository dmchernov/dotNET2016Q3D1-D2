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
        Int32 AddOrder(Order order);
        Order GetOrderById(Int32 id, Boolean withDetails);
        Boolean DeleteOrder(Order order);
        Order ProcessOrder(Order order, DateTime orderDate);
        Order CompleteOrder(Order order, DateTime shippedDate);
        Boolean ChangeOrder(Order order);
    }
}
