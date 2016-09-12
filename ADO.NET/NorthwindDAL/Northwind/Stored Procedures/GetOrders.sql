CREATE PROCEDURE [dbo].[GetOrders]
AS
BEGIN
	SELECT	OrderID,
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
	FROM dbo.Orders
END
