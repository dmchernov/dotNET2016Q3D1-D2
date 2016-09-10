/*Версия 1.1. Добавляет таблицу данных кредитных карт сотрудников: номер карты, дата истечения, имя card holder, ссылку на сотрудника*/
USE Northwind
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('dbo.EmployeeCreditCards'))
CREATE TABLE dbo.EmployeeCreditCards(
	ID					INT NOT NULL PRIMARY KEY IDENTITY,
	CardNumber			NVARCHAR(12)	NOT NULL,
	ExpirationDate		DATETIME		NOT NULL,
	CardHolder			NVARCHAR(30)	NOT NULL,
	EmployeeID			INT				NULL
	CONSTRAINT FK_EmployeeCreditCards_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees (EmployeeID))