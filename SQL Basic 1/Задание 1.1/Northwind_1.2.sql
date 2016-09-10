/*Версия 1.2. Добавляет следующие минорные изменения относительно 1.1:
	Переименование Region в Regions
	Добавление в таблицу клиентов даты основания*/

USE Northwind
GO
IF EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.Region'))
EXEC sp_rename 'dbo.Region', 'Regions';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Customers') AND name = 'FoundationDate')
	ALTER TABLE dbo.Customers
		ADD FoundationDate DATETIME
	GO