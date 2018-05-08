CREATE TABLE [dbo].[AuditRequestCode] (
    [AuditRequestCodeId] INT          IDENTITY (1, 1) NOT NULL,
    [Code]               VARCHAR (50) NOT NULL,
    [Description]        VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AuditRequestCode] PRIMARY KEY CLUSTERED ([AuditRequestCodeId] ASC)
);

