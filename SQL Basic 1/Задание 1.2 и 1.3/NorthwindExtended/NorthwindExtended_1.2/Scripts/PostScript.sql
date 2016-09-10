USE NorthwindExtended
GO

-- Categories
IF OBJECT_ID('tempdb..#table') IS NOT NULL
	DROP TABLE #table
GO

CREATE TABLE #table
	([CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](15) NOT NULL,
	[Description] [ntext] NULL,
	[Picture] [image] NULL)
GO

INSERT INTO #table
VALUES (1,'Beverages','Soft drinks, coffees, teas, beers, and ales', NULL),
	   (2,'Condiments','Sweet and savory sauces, relishes, spreads, and seasonings', NULL),
	   (3,'Confections','Desserts, candies, and sweet breads', NULL),
	   (4,'Dairy Products','Cheeses', NULL),
	   (5,'Grains/Cereals','Breads, crackers, pasta, and cereal', NULL),
	   (6,'Meat/Poultry','Prepared meats', NULL),
	   (7,'Produce','Dried fruit and bean curd', NULL),
	   (8,'Seafood','Seaweed and fish', NULL)
GO

SET QUOTED_IDENTIFIER ON
GO

SET IDENTITY_INSERT Categories ON
GO

ALTER TABLE Categories NOCHECK CONSTRAINT ALL
GO

MERGE dbo.Categories cat
USING #table tbl
ON cat.CategoryID = tbl.CategoryID
WHEN NOT MATCHED BY TARGET THEN
INSERT ([CategoryID], [CategoryName], [Description], [Picture])
VALUES (tbl.[CategoryID], tbl.[CategoryName], tbl.[Description], tbl.[Picture]);
GO

SET IDENTITY_INSERT Categories OFF
GO

ALTER TABLE Categories CHECK CONSTRAINT ALL
GO

DROP TABLE #table
GO

-- Suppliers
IF OBJECT_ID('tempdb..#table') IS NOT NULL
	DROP TABLE #table
GO

CREATE TABLE #table
	([SupplierID] [int] NOT NULL,
	[CompanyName] [nvarchar](40) NOT NULL,
	[ContactName] [nvarchar](30) NULL,
	[ContactTitle] [nvarchar](30) NULL,
	[Address] [nvarchar](60) NULL,
	[City] [nvarchar](15) NULL,
	[Region] [nvarchar](15) NULL,
	[PostalCode] [nvarchar](10) NULL,
	[Country] [nvarchar](15) NULL,
	[Phone] [nvarchar](24) NULL,
	[Fax] [nvarchar](24) NULL,
	[HomePage] [ntext] NULL)
GO



INSERT INTO #table
VALUES (1,'Exotic Liquids','Charlotte Cooper','Purchasing Manager','49 Gilbert St.','London',NULL,'EC1 4SD','UK','(171) 555-2222',NULL,NULL), -- таблица источник
	   (2,'New Orleans Cajun Delights','Shelley Burke','Order Administrator','P.O. Box 78934','New Orleans','LA','70117','USA','(100) 555-4822',NULL,'#CAJUN.HTM#'),
	   (3,'Grandma Kelly''s Homestead','Regina Murphy','Sales Representative','707 Oxford Rd.','Ann Arbor','MI','48104','USA','(313) 555-5735','(313) 555-3349',NULL),
	   (4,'Tokyo Traders','Yoshi Nagase','Marketing Manager','9-8 Sekimai Musashino-shi','Tokyo',NULL,'100','Japan','(03) 3555-5011',NULL,NULL),
	   (5,'Cooperativa de Quesos ''Las Cabras''','Antonio del Valle Saavedra','Export Administrator','Calle del Rosal 4','Oviedo','Asturias','33007','Spain','(98) 598 76 54',NULL,NULL),
	   (6,'Mayumi''s','Mayumi Ohno','Marketing Representative','92 Setsuko Chuo-ku','Osaka',NULL,'545','Japan','(06) 431-7877',NULL,'Mayumi''s (on the World Wide Web)#http://www.microsoft.com/accessdev/sampleapps/mayumi.htm#'),
	   (7,'Pavlova, Ltd.','Ian Devling','Marketing Manager','74 Rose St. Moonie Ponds','Melbourne','Victoria','3058','Australia','(03) 444-2343','(03) 444-6588',NULL),
	   (8,'Specialty Biscuits, Ltd.','Peter Wilson','Sales Representative','29 King''s Way','Manchester',NULL,'M14 GSD','UK','(161) 555-4448',NULL,NULL),
	   (9,'PB Knäckebröd AB','Lars Peterson','Sales Agent','Kaloadagatan 13','Göteborg',NULL,'S-345 67','Sweden','031-987 65 43','031-987 65 91',NULL),
	   (10,'Refrescos Americanas LTDA','Carlos Diaz','Marketing Manager','Av. das Americanas 12.890','Sao Paulo',NULL,'5442','Brazil','(11) 555 4640',NULL,NULL);
GO

set quoted_identifier on
go

set identity_insert Suppliers on
go

ALTER TABLE Suppliers NOCHECK CONSTRAINT ALL
go

MERGE dbo.Suppliers supp
USING #table tbl
ON supp.SupplierID = tbl.SupplierID
WHEN NOT MATCHED BY TARGET THEN
INSERT ([SupplierID], [CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country], [Phone], [Fax], [HomePage])
VALUES (tbl.[SupplierID], tbl.[CompanyName], tbl.[ContactName], tbl.[ContactTitle], tbl.[Address], tbl.[City], tbl.[Region], tbl.[PostalCode], tbl.[Country], tbl.[Phone], tbl.[Fax], tbl.[HomePage]);
GO

SET IDENTITY_INSERT Suppliers OFF
GO

ALTER TABLE Suppliers CHECK CONSTRAINT ALL
GO

DROP TABLE #table
GO

-- Products
IF OBJECT_ID('tempdb..#table') IS NOT NULL
	DROP TABLE #table
GO

CREATE TABLE #table
	([ProductID] [int] NOT NULL,
	[ProductName] [nvarchar](40) NOT NULL,
	[SupplierID] [int] NULL,
	[CategoryID] [int] NULL,
	[QuantityPerUnit] [nvarchar](20) NULL,
	[UnitPrice] [money] NULL CONSTRAINT [DF_Products_UnitPrice]  DEFAULT ((0)),
	[UnitsInStock] [smallint] NULL CONSTRAINT [DF_Products_UnitsInStock]  DEFAULT ((0)),
	[UnitsOnOrder] [smallint] NULL CONSTRAINT [DF_Products_UnitsOnOrder]  DEFAULT ((0)),
	[ReorderLevel] [smallint] NULL CONSTRAINT [DF_Products_ReorderLevel]  DEFAULT ((0)),
	[Discontinued] [bit] NOT NULL CONSTRAINT [DF_Products_Discontinued]  DEFAULT ((0)))
GO

INSERT INTO #table
VALUES (1,'Chai',1,1,'10 boxes x 20 bags',18,39,0,10,0),
	   (2,'Chang',1,1,'24 - 12 oz bottles',19,17,40,25,0),
	   (3,'Aniseed Syrup',1,2,'12 - 550 ml bottles',10,13,70,25,0),
	   (4,'Chef Anton''s Cajun Seasoning',2,2,'48 - 6 oz jars',22,53,0,0,0),
	   (5,'Chef Anton''s Gumbo Mix',2,2,'36 boxes',21.35,0,0,0,1),
	   (6,'Grandma''s Boysenberry Spread',3,2,'12 - 8 oz jars',25,120,0,25,0),
	   (7,'Uncle Bob''s Organic Dried Pears',3,7,'12 - 1 lb pkgs.',30,15,0,10,0),
	   (8,'Northwoods Cranberry Sauce',3,2,'12 - 12 oz jars',40,6,0,0,0),
	   (9,'Mishi Kobe Niku',4,6,'18 - 500 g pkgs.',97,29,0,0,1),
	   (10,'Ikura',4,8,'12 - 200 ml jars',31,31,0,0,0)
GO

SET QUOTED_IDENTIFIER ON
GO

SET IDENTITY_INSERT Products ON
GO

ALTER TABLE Products NOCHECK CONSTRAINT ALL
GO

MERGE dbo.Products prod
USING #table tbl
ON prod.ProductID = tbl.ProductID
WHEN NOT MATCHED BY TARGET THEN
INSERT ([ProductID], [ProductName], [SupplierID], [CategoryID], [QuantityPerUnit], [UnitPrice], [UnitsInStock], [UnitsOnOrder], [ReorderLevel], [Discontinued])
VALUES (tbl.[ProductID], tbl.[ProductName], tbl.[SupplierID], tbl.[CategoryID], tbl.[QuantityPerUnit], tbl.[UnitPrice], tbl.[UnitsInStock], tbl.[UnitsOnOrder], tbl.[ReorderLevel], tbl.[Discontinued]);
GO

SET IDENTITY_INSERT Products OFF
GO

ALTER TABLE Products CHECK CONSTRAINT ALL
GO

DROP TABLE #table
GO