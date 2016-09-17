using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindDAL.Interfaces;
using NorthwindDAL.Models;

namespace NorthwindDAL.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;

        public StatisticRepository(string conString, string provider)
        {
            providerFactory = DbProviderFactories.GetFactory(provider);
            connectionString = conString;
        }

        public List<ProductsTotal> GetCustomersProducts(String customerId)
        {
            List<ProductsTotal> products = new List<ProductsTotal>();
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CustOrderHist";
                    
                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@CustomerID";
                    paramId.Value = customerId;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductsTotal() {Product = reader.GetValue(0) as String, Total = (Int32)reader.GetValue(1)});
                        }
                    }
                }
            }

            return products;
        }

        public List<OrderDetailsExtended> GetCustOrdersDetail(Int32 id)
        {
            List<OrderDetailsExtended> details = new List<OrderDetailsExtended>();
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CustOrdersDetail";

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@OrderID";
                    paramId.Value = id;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var det = new OrderDetailsExtended();
                            det.Product = reader.GetValue(0) as String;
                            det.UnitPrice = (Decimal) reader.GetValue(1);
                            det.Quantity = (Int16) reader.GetValue(2);
                            det.Discount = (Int32) reader.GetValue(3);
                            det.ExtendedPrice = (Decimal) reader.GetValue(4);
                            details.Add(det);
                            //{
                            //    Product = reader.GetValue(0) as String, UnitPrice = (Decimal)reader.GetValue(1),
                            //    Quantity = (Int32)reader.GetValue(2), Discount = (Int32)reader.GetValue(3),
                            //    ExtendedPrice = (Decimal)reader.GetValue(4)
                            //});
                        }
                    }
                }
            }

            return details;
        }
    }
}
