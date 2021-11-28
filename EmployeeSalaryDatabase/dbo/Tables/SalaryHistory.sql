CREATE TABLE [dbo].[SalaryHistory] (
    [SalaryID]     INT            IDENTITY (1, 1) NOT NULL,
    [EmpId]        INT            NULL,
    [Month]        NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [Year]         INT            NOT NULL,
    [SalaryAmount] FLOAT (53)     NOT NULL DEFAULT 0,
    [GrossSalary] FLOAT NULL DEFAULT 0, 
    [IncomeTax] FLOAT NULL DEFAULT 0, 
    [PensionAmount] FLOAT NULL DEFAULT 0, 
    [LabourTaxCredit] FLOAT NULL DEFAULT 0, 
    [WorkingHours] INT NULL DEFAULT 40, 
    CONSTRAINT [PK_SalaryHistory] PRIMARY KEY CLUSTERED ([SalaryID] ASC),
    CONSTRAINT [FK_SalaryHistory_Employees_EmpId] FOREIGN KEY ([EmpId]) REFERENCES [dbo].[Employees] ([EmpID])
);


GO
CREATE NONCLUSTERED INDEX [IX_SalaryHistory_EmpId]
    ON [dbo].[SalaryHistory]([EmpId] ASC);

