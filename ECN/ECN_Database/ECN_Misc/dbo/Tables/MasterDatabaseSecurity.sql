CREATE TABLE [dbo].[MasterDatabaseSecurity] (
    [UserID]             INT           NOT NULL,
    [UserRoles]          VARCHAR (200) NULL,
    [ExportToCustomerID] VARCHAR (500) NULL,
    CONSTRAINT [PK_MasterDatabaseSecurity] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (FILLFACTOR = 80)
);

