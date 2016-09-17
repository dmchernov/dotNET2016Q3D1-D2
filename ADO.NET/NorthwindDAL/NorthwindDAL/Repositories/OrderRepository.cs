using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NorthwindDAL.Enums;
using NorthwindDAL.Interfaces;
using NorthwindDAL.Models;

namespace NorthwindDAL.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DbProviderFactory providerFactory;
		private readonly string connectionString;

		private const String GET_ORDER = @"SELECT	OrderID,
													OrderDate,
													ShippedDate,
													Status = CASE
																WHEN OrderDate IS NULL AND ShippedDate IS NULL THEN 0
																WHEN OrderDate IS NOT NULL AND ShippedDate IS NULL THEN 1
																WHEN ShippedDate IS NOT NULL AND OrderDate IS NOT NULL THEN 2
															 END,
													Freight,
													ShipName,
													ShipAddress,
													ShipCity,
													ShipRegion,
													ShipPostalCode,
													ShipCountry
											FROM dbo.Orders";

		public OrderRepository(string conString, string provider)
		{
			providerFactory = DbProviderFactories.GetFactory(provider);
			connectionString = conString;
		}

		public IEnumerable<Order> GetOrders()
		{
			var resultOrders = new List<Order>();

			using (var connection = providerFactory.CreateConnection())
			{
				connection.ConnectionString = connectionString;
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = GET_ORDER;
					command.CommandType = CommandType.Text;
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							resultOrders.Add(ReaderToOrder(reader));
						}
					}
				}
			}

			return resultOrders;
		}

		public Int32 AddOrder(Order order)
		{
			using (var connection = providerFactory.CreateConnection())
			{
				connection.ConnectionString = connectionString;
				connection.Open();
                var command = connection.CreateCommand();
			    try
			    {
			        command.CommandText = @"INSERT INTO dbo.Orders (Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
											VALUES (@freight, @shipName, @shipAddress, @shipCity, @shipRegion, @shipPostalCode, @shipCountry); 
											DECLARE @id DECIMAL = SCOPE_IDENTITY(); "
			                              +
			                              ((order.OrderDetails != null && order.OrderDetails.Count > 0)
			                                  ? @"EXEC dbo.SaveOrderDetails @id, @xmlData; "
			                                  : "") +
			                              @"SELECT @id;";
			        command.CommandType = CommandType.Text;

			        PrepareOrderParameters(order, ref command);

                    if (order.OrderDetails != null && order.OrderDetails.Count > 0)
			        {
			            var paramData = command.CreateParameter();
			            paramData.Value = PrepareOrderDetails(order);
			            paramData.ParameterName = "@xmlData";
			            command.Parameters.Add(paramData);
			        }

			        var insertedId = (Int32) (Decimal) command.ExecuteScalar();

			        return insertedId;
			    }
			    finally
			    {
			        command.Dispose();
			    }
			}
		}

        private String PrepareOrderDetails(Order order)
        {
            XDocument details = new XDocument();
            XElement detailsElement = new XElement("Details");
            
            foreach (var orderDetail in order.OrderDetails)
            {
                XElement detElement = new XElement("Detail", new XAttribute("ProductId", orderDetail.ProductId), new XAttribute("Price", orderDetail.UnitPrice),
                    new XAttribute("Quantity", orderDetail.Quantity), new XAttribute("Discount", orderDetail.Discount));
                detailsElement.Add(detElement);
            }

            details.Add(detailsElement);
            return details.ToString();
        }

        public Order GetOrderById(Int32 id, Boolean withDetails)
		{
			using (var connection = providerFactory.CreateConnection())
			{
				connection.ConnectionString = connectionString;
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = GET_ORDER + @" WHERE OrderID = @id;";
					if (withDetails)
						command.CommandText += @"	SELECT	orDet.ProductID,
															prod.ProductName,
															orDet.UnitPrice,
															orDet.Quantity,
															orDet.Discount
													FROM	[Order Details] orDet
															INNER JOIN dbo.Products prod ON orDet.ProductID = prod.ProductID
													WHERE   orDet.OrderID = @id;";
					command.CommandType = CommandType.Text;

					var paramId = command.CreateParameter();
					paramId.ParameterName = "@id";
					paramId.Value = id;

					command.Parameters.Add(paramId);

					using (var reader = command.ExecuteReader())
					{
						if (!reader.HasRows) return null;

						reader.Read();

						var order = ReaderToOrder(reader);

						if (withDetails)
						{
							reader.NextResult();
                            List<OrderDetails> detailsList = new List<OrderDetails>();

							while (reader.Read())
							{
								var details = new OrderDetails();
								details.ProductId = (Int32) reader["ProductID"];
								details.Product = (String) reader["ProductName"];
								details.UnitPrice = (Decimal)reader["UnitPrice"];
								details.Quantity = (Int16)reader["Quantity"];
								details.Discount = (Decimal)(float)reader["Discount"];

								detailsList.Add(details);
							}
						    order.OrderDetails = detailsList;
						}

						return order;
					}
				}
			}
		}

		public bool DeleteOrder(Order order)
		{
		    if (order.Status == Status.Success) return false;

            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"IF EXISTS (SELECT 1 FROM dbo.Orders WHERE OrderID = @id)
                                            BEGIN
                                            DELETE FROM dbo.[Order Details] WHERE OrderID = @id;
                                            DELETE FROM dbo.Orders WHERE OrderID = @id;
                                            SELECT 1
                                            END
                                            ELSE SELECT 0";
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = order.OrderId;

                    command.Parameters.Add(paramId);

                    return ((Int32)command.ExecuteScalar()) == 1 ? true : false;
                }
            }
        }

		public Order ProcessOrder(Order order, DateTime orderDate)
		{
		    if (order.Status != Status.New) return null;

            return ChangeOrderStatus(order, orderDate, Status.Process);
        }

		public Order CompleteOrder(Order order, DateTime shippedDate)
		{
		    if (order.Status != Status.Process) return null;

		    return ChangeOrderStatus(order, shippedDate, Status.Success);
		}

	    public Boolean ChangeOrder(Order order)
	    {
            if (order.Status != Status.New) return false;

	        if (GetOrderById(order.OrderId, false) == null) return false;

            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                var command = connection.CreateCommand();
                try
                {
                    command.CommandText = @"DELETE FROM dbo.[Order Details] WHERE OrderID = @id; " +
                                           "UPDATE dbo.Orders SET   Freight = @freight," +
                                                                   "ShipName = @shipName," +
                                                                   "ShipAddress = @shipAddress," +
                                                                   "ShipCity = @shipCity," +
                                                                   "ShipRegion = @shipRegion," +
                                                                   "ShipPostalCode = @shipPostalCode," +
                                                                   "ShipCountry = @shipCountry " +
                                          "WHERE OrderID = @id; " +
                                          "EXEC dbo.SaveOrderDetails @id, @xmlData;";
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = order.OrderId;
                    command.Parameters.Add(paramId);

                    var paramData = command.CreateParameter();
                    paramData.Value = (order.OrderDetails != null && order.OrderDetails.Count > 0) ? PrepareOrderDetails(order) : "";
                    paramData.ParameterName = "@xmlData";
                    command.Parameters.Add(paramData);

                    PrepareOrderParameters(order, ref command);

                    command.ExecuteScalar();

                    return true;
                }
                finally
                {
                    command.Dispose();
                }
            }
        }

	    private Order ReaderToOrder(IDataReader reader)
		{
			return new Order(reader.GetInt32(0), reader.GetValue(1) as DateTime?, reader.GetValue(2) as DateTime?, reader.GetValue(4) as Decimal?, reader.GetValue(5) as String, reader.GetValue(6) as String, reader.GetValue(7) as String, reader.GetValue(8) as String, reader.GetValue(9) as String, reader.GetValue(10) as String);
		}

	    private Order ChangeOrderStatus(Order order, DateTime date, Status newStatus)
	    {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE dbo.Orders SET " + (newStatus == Status.Process ? "OrderDate" : "ShippedDate") + " = @date WHERE OrderID = @id;";
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = order.OrderId;

                    var paramDate = command.CreateParameter();
                    paramDate.ParameterName = "@date";
                    paramDate.Value = date;

                    command.Parameters.AddRange(new DbParameter[] { paramId, paramDate });
                    var count = command.ExecuteNonQuery();
                    if (count == 0) return null;
                    else if (count == 1) return GetOrderById(order.OrderId, order.OrderDetails != null && order.OrderDetails.Count > 0);
                    else throw new Exception("Запрос обработал неверное количество строк");
                }
            }
        }

	    private void PrepareOrderParameters(Order order, ref DbCommand command)
	    {
            var paramFreight = command.CreateParameter();
            paramFreight.ParameterName = "@freight";
            paramFreight.Value = order.Freight.HasValue ? (object)order.Freight : DBNull.Value;
            command.Parameters.Add(paramFreight);

            var paramShipName = command.CreateParameter();
            paramShipName.ParameterName = "@shipName";
            paramShipName.Value = !String.IsNullOrEmpty(order.ShipName) ? (object)order.ShipName : DBNull.Value;
            command.Parameters.Add(paramShipName);

            var paramShipAddress = command.CreateParameter();
            paramShipAddress.ParameterName = "@shipAddress";
            paramShipAddress.Value = !String.IsNullOrEmpty(order.ShipAddress) ? (object)order.ShipAddress : DBNull.Value;
            command.Parameters.Add(paramShipAddress);

            var paramShipCity = command.CreateParameter();
            paramShipCity.ParameterName = "@shipCity";
            paramShipCity.Value = !String.IsNullOrEmpty(order.ShipCity) ? (object)order.ShipCity : DBNull.Value;
            command.Parameters.Add(paramShipCity);

            var paramShipRegion = command.CreateParameter();
            paramShipRegion.ParameterName = "@shipRegion";
            paramShipRegion.Value = !String.IsNullOrEmpty(order.ShipRegion) ? (object)order.ShipRegion : DBNull.Value;
            command.Parameters.Add(paramShipRegion);

            var paramShipPostalCode = command.CreateParameter();
            paramShipPostalCode.ParameterName = "@shipPostalCode";
            paramShipPostalCode.Value = !String.IsNullOrEmpty(order.ShipPostalCode) ? (object)order.ShipPostalCode : DBNull.Value;
            command.Parameters.Add(paramShipPostalCode);

            var paramShipCountry = command.CreateParameter();
            paramShipCountry.ParameterName = "@shipCountry";
            paramShipCountry.Value = !String.IsNullOrEmpty(order.ShipCountry) ? (object)order.ShipCountry : DBNull.Value;
            command.Parameters.Add(paramShipCountry);
        }
	}
}