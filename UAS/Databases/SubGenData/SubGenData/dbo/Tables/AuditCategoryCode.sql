CREATE TABLE [dbo].[AuditCategoryCode] (
    [AuditCatecoryCodeId] INT          IDENTITY (1, 1) NOT NULL,
    [Code]                VARCHAR (50) NOT NULL,
    [Description]         VARCHAR (50) NOT NULL,
    [KMCategoryCodeId]    INT          NULL
);

