CREATE TABLE [dbo].[EmployeeCreditCards] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [CardNumber]     NVARCHAR (12) NOT NULL,
    [ExpirationDate] DATETIME      NOT NULL,
    [CardHolder]     NVARCHAR (30) NOT NULL,
    [EmployeeID]     INT           NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),

    CONSTRAINT [FK_EmployeeCreditCards_Emloyees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

