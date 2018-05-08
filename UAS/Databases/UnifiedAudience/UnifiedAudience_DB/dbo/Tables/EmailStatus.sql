CREATE TABLE [dbo].[EmailStatus] (
    [EmailStatusID] INT          IDENTITY (1, 1) NOT NULL,
    [Status]        VARCHAR (50) NULL,
    CONSTRAINT [PK_EmailStatus] PRIMARY KEY CLUSTERED ([EmailStatusID] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_EmailStatus_Status]
    ON [dbo].[EmailStatus]([Status] ASC) WITH (FILLFACTOR = 90);
GO

