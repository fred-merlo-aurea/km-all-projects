CREATE TABLE [dbo].[UserDepartments] (
    [UserDepartmentID] INT IDENTITY (1, 1) NOT NULL,
    [UserID]           INT NOT NULL,
    [DepartmentID]     INT NOT NULL,
    [IsDefaultDept]    BIT CONSTRAINT [DF_UserDepartments_IsDefaultDept] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserDepartments] PRIMARY KEY CLUSTERED ([UserDepartmentID] ASC) WITH (FILLFACTOR = 80)
);

