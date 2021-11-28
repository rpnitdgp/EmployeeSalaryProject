CREATE TABLE [dbo].[EmployeeSalary]
(
	[Id] INT    IDENTITY (1, 1) NOT NULL, 
    [EmpId] INT NULL, 
    [NetSalary] FLOAT NULL, 
    [IncomeTax] FLOAT NULL, 
    [PensionAmount] FLOAT NULL, 
    [LabourTaxCredit] FLOAT NULL, 
    CONSTRAINT [FK_EmployeeSalary_Employees] FOREIGN KEY ([EmpId]) REFERENCES [dbo].[Employees]([EmpId])
)
