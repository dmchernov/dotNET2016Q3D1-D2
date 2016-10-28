using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;

namespace Task.Serialization
{
	public class OrderDetailsSerializationSurrogate : ISerializationSurrogate
	{
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			var o = (Order_Detail)obj;
			info.AddValue("Order", o.Order);
			info.AddValue("Product", o.Product);
			info.AddValue("UnitPrice", o.UnitPrice);
			info.AddValue("Quantity", o.Quantity);
			info.AddValue("Discount", o.Discount);
		}

		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			var od = obj as Order_Detail;
			if (od != null)
			{
				od.Product = info.GetValue("Product", typeof(Product)) as Product;
				od.Order = info.GetValue("Order", typeof(Order)) as Order;
				od.UnitPrice = info.GetDecimal("UnitPrice");
				od.Quantity = info.GetInt16("Quantity");
				od.Discount = info.GetSingle("Discount");
			}
			return od;
		}
	}
}
