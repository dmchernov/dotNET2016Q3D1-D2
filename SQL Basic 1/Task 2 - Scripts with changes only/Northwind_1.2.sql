/*Версия 1.2. Добавляет следующие минорные изменения относительно 1.1:
	Переименование Region в Regions
	Добавление в таблицу клиентов даты основания*/

USE Northwind
GO
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Region')
EXEC sp_rename 'dbo.Region', 'Regions';
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'FoundationDate')
	ALTER TABLE dbo.Customers
		ADD FoundationDate DATETIME
	GO