CREATE TABLE [dbo].[History] (
    [HistoryID]             INT      IDENTITY (1, 1) NOT NULL,
    [BatchID]               INT      NOT NULL,
    [BatchCountItem]        INT      NOT NULL,
    [PublicationID]         INT      NOT NULL,
    [PubSubscriptionID]		INT      NOT NULL,
    [SubscriptionID]        INT      NOT NULL,
    [HistorySubscriptionID] INT      NULL,
    [HistoryPaidID]         INT      NULL,
    [HistoryPaidBillToID]   INT      NULL,
    [DateCreated]           DATETIME NOT NULL,
    [CreatedByUserID]       INT      NOT NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([HistoryID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IX_History_BatchId]
	ON [dbo].[History] ([BatchID])
GO

CREATE NONCLUSTERED INDEX [IX_History_PubSubscriptionID]
    ON [dbo].[History]([PubSubscriptionID] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_History_SubscriptionID]
    ON [dbo].[History]([SubscriptionID] ASC);
GO
