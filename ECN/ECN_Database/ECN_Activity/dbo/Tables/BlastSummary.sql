CREATE TABLE [dbo].[BlastSummary] (
    [BlastSummaryID] INT          IDENTITY (1, 1) NOT NULL,
    [BlastID]        INT          NOT NULL,
    [DistinctTotal]  INT          NULL,
    [Total]          INT          NULL,
    [ActionTypeCode] VARCHAR (50) NULL,
    CONSTRAINT [PK_BlastSummary] PRIMARY KEY CLUSTERED ([BlastSummaryID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastSummary_BID_ATC]
    ON [dbo].[BlastSummary]([BlastID] ASC, [ActionTypeCode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_BlastSummary_BID_ATC]
    ON [dbo].[BlastSummary]([BlastID] ASC, [ActionTypeCode] ASC) WITH (FILLFACTOR = 80);

