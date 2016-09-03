CREATE TABLE [dbo].[EmployeeCreditCards]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CardNumber] NVARCHAR(12) NOT NULL, 
    [ExpirationDate] DATETIME NOT NULL, 
    [CardHolder] NVARCHAR(30) NOT NULL, 
    [EmployeeID] INT NULL, 
    CONSTRAINT [FK_EmployeeCreditCards_Emloyees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees]([EmployeeID]) 
)
