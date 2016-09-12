using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Decimal AddOrder(Order order)
		{
			using (var connection = providerFactory.CreateConnection())
			{
				connection.ConnectionString = connectionString;
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = @"INSERT INTO dbo.Orders (Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
											VALUES (@freight, @shipName, @shipAddress, @shipCity, @shipRegion, @shipPostalCode, @shipCountry); 
											SELECT SCOPE_IDENTITY()";
					command.CommandType = CommandType.Text;

					var paramFreight = command.CreateParameter();
					paramFreight.ParameterName = "@freight";
					paramFreight.Value = order.Freight.HasValue ? (object) order.Freight : DBNull.Value;
					command.Parameters.Add(paramFreight);

					var paramShipName = command.CreateParameter();
					paramShipName.ParameterName = "@shipName";
					paramShipName.Value = !String.IsNullOrEmpty(order.ShipName) ? (object) order.ShipName : DBNull.Value;
					command.Parameters.Add(paramShipName);

					var paramShipAddress = command.CreateParameter();
					paramShipAddress.ParameterName = "@shipAddress";
					paramShipAddress.Value = !String.IsNullOrEmpty(order.ShipAddress) ? (object) order.ShipAddress : DBNull.Value;
					command.Parameters.Add(paramShipAddress);

					var paramShipCity = command.CreateParameter();
					paramShipCity.ParameterName = "@shipCity";
					paramShipCity.Value = !String.IsNullOrEmpty(order.ShipCity) ? (object) order.ShipCity : DBNull.Value;
					command.Parameters.Add(paramShipCity);

					var paramShipRegion = command.CreateParameter();
					paramShipRegion.ParameterName = "@shipRegion";
					paramShipRegion.Value = !String.IsNullOrEmpty(order.ShipRegion) ? (object) order.ShipRegion : DBNull.Value;
					command.Parameters.Add(paramShipRegion);

					var paramShipPostalCode = command.CreateParameter();
					paramShipPostalCode.ParameterName = "@shipPostalCode";
					paramShipPostalCode.Value = !String.IsNullOrEmpty(order.ShipPostalCode) ? (object) order.ShipPostalCode : DBNull.Value;
					command.Parameters.Add(paramShipPostalCode);

					var paramShipCountry = command.CreateParameter();
					paramShipCountry.ParameterName = "@shipCountry";
					paramShipCountry.Value = !String.IsNullOrEmpty(order.ShipCountry) ? (object) order.ShipCountry : DBNull.Value;
					command.Parameters.Add(paramShipCountry);

					return (Decimal)command.ExecuteScalar();
				}
			}
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
							order.OrderDetails = new List<OrderDetails>();

							while (reader.Read())
							{
								var details = new OrderDetails();
								details.ProductId = (Int32) reader["ProductID"];
								details.Product = (String) reader["ProductName"];
								details.Price = (Decimal)reader["UnitPrice"];
								details.Quantity = (Int32)reader["Quantity"];
								details.Discount = (Decimal)reader["Discount"];

								order.OrderDetails.Add(details);
							}
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

		public int ProcessOrder(Int32 id, DateTime orderDate)
		{
			throw new NotImplementedException();
		}

		public int CompleteOrder(Int32 id, DateTime shippedDate)
		{
			throw new NotImplementedException();
		}

		private Order ReaderToOrder(IDataReader reader)
		{
			return new Order(reader.GetInt32(0), reader.GetValue(1) as DateTime?, reader.GetValue(2) as DateTime?, reader.GetValue(4) as Decimal?, reader.GetValue(5) as String, reader.GetValue(6) as String, reader.GetValue(7) as String, reader.GetValue(8) as String, reader.GetValue(9) as String, reader.GetValue(10) as String);
		}
	}
}
