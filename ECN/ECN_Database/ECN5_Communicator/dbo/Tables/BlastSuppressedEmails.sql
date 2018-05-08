CREATE TABLE [dbo].[BlastSuppressedEmails] (
    [BlastSuppressionRecID] INT      IDENTITY (1, 1) NOT NULL,
    [BlastID]               INT      NULL,
    [EmailID]               INT      NULL,
    [DateAdded]             DATETIME NULL,
    CONSTRAINT [PK_BlastEmailsIgnored] PRIMARY KEY CLUSTERED ([BlastSuppressionRecID] ASC) WITH (FILLFACTOR = 80)
);

