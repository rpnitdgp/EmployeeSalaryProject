CREATE TABLE [dbo].[Employees] (
    [EmpID]       INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (MAX) NULL,
    [LastName]    NVARCHAR (MAX) NULL,
    [DateOfBirth] DATETIME2 (7)  DEFAULT ('1900-01-01T00:00:00.0000000') NOT NULL,
    [Age] INT NULL, 
    [WorkingHours] INT NULL, 
    [GrossSalary] FLOAT NULL, 
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([EmpID] ASC)
);

