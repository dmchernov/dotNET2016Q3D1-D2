using System;
using System.Collections.Generic;
using NorthwindDAL.Enums;

namespace NorthwindDAL.Models
{
    public class Order
    {
        public Order() { }

        public Order(Int32 id, DateTime? orderDate, DateTime? shippedDate, Decimal? freight, String sName,
            String sAddress, String sCity, String sRegion, String sPostalCode, String sCountry)
        {
            OrderId = id;
            OrderDate = orderDate;
            ShippedDate = shippedDate;
            Freight = freight;
            ShipName = sName;
            ShipAddress = sAddress;
            ShipCity = sCity;
            ShipRegion = sRegion;
            ShipPostalCode = sPostalCode;
            ShipCountry = sCountry;
        }
        public Int32 OrderId { get; private set; }
        public DateTime? OrderDate { get; private set; }

        private DateTime? _shippedDate;
        public DateTime? ShippedDate
        {
            get { return _shippedDate; }
            private set
            {
                if (OrderDate.HasValue) _shippedDate = value;
            }
        }

        public Status Status
        {
            get
            {
                return (OrderDate.HasValue && ShippedDate.HasValue)
                    ? Status.Success
                    : (OrderDate.HasValue && !ShippedDate.HasValue) ? Status.Process : Status.New;
            }
        }

        private Decimal? _freight;
        public Decimal? Freight
        {
            get { return _freight; }
            set { if (Status == Status.New) _freight = value; }
        }

        private String _shipName;
        public String ShipName
        {
            get { return _shipName; }
            set { if(Status == Status.New) _shipName = value; }
        }

        private String _shipAddress;
        public String ShipAddress
        {
            get { return _shipAddress; }
            set { if (Status == Status.New) _shipAddress = value; }
        }

        private String _shipCity;
        public String ShipCity
        {
            get { return _shipCity; }
            set { if (Status == Status.New) _shipCity = value; }
        }

        private String _shipRegion;
        public String ShipRegion
        {
            get { return _shipRegion; }
            set { if (Status == Status.New) _shipRegion = value; }
        }

        private String _shipPostalCode;
        public String ShipPostalCode
        {
            get { return _shipPostalCode; }
            set { if (Status == Status.New) _shipPostalCode = value; }
        }

        private String _shipCountry;
        public String ShipCountry
        {
            get { return _shipCountry; }
            set { if (Status == Status.New) _shipCountry = value; }
        }

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
