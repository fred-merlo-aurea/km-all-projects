CREATE TABLE [dbo].[Severity] (
    [SeverityID]          INT           NOT NULL,
    [SeverityName]        NVARCHAR (50) NULL,
    [SeverityDescription] TEXT          NULL,
    CONSTRAINT [PK_Severity] PRIMARY KEY CLUSTERED ([SeverityID] ASC) WITH (FILLFACTOR = 80)
);

