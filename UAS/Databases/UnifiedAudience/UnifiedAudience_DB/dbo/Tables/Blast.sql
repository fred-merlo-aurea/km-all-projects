CREATE TABLE [dbo].[Blast] (
    [BlastID]         INT           NOT NULL,
    [CustomerID]      INT           NULL,
    [EmailSubject]    VARCHAR (255) NULL,
    [EmailFrom]       VARCHAR (100) NULL,
    [EmailFromName]   VARCHAR (100) NULL,
    [SendTime]        DATETIME      NULL,
    [StatusCode]      VARCHAR (50)  NULL,
    [BlastType]       VARCHAR (50)  NULL,
    [DateCreated]     DATETIME      CONSTRAINT [DF_Blast_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NULL,
    [UpdatedByUserID] INT           NULL,
    [ECNCampaignId]   INT           NULL,
    CONSTRAINT [PK_Blast] PRIMARY KEY CLUSTERED ([BlastID] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE INDEX [IDX_Blast_SendTime] ON [dbo].[Blast] ([SendTime]) INCLUDE ([BlastID], [ECNCampaignId])

GO

CREATE INDEX [IDX_Blast_ECNCampaignId_BlastID] ON [dbo].[Blast] ([ECNCampaignId], [BlastID])

GO
CREATE STATISTICS [STAT_Blast_BlastID_ECNCampaignId] ON [dbo].[Blast] ([BlastID], [ECNCampaignId])

