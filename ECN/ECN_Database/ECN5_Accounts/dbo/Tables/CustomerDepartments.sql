CREATE TABLE [dbo].[CustomerDepartments] (
    [DepartmentID]   INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]     INT           NULL,
    [DepartmentName] VARCHAR (50)  NULL,
    [DepartmentDesc] VARCHAR (255) NULL,
    CONSTRAINT [PK_CustomerDepartments] PRIMARY KEY CLUSTERED ([DepartmentID] ASC) WITH (FILLFACTOR = 80)
);

