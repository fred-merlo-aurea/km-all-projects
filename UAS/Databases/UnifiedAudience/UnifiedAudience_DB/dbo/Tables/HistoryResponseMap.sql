CREATE TABLE [dbo].[HistoryResponseMap] (
    [HistoryResponseMapID] INT           IDENTITY (1, 1) NOT NULL,
	[PubSubscriptionDetailID]       INT           NOT NULL,
	[PubSubscriptionID]       INT           NOT NULL,
    [SubscriptionID]       INT           NOT NULL,
    [CodeSheetID]           INT           NOT NULL,
    [IsActive]             BIT           NOT NULL,
    [DateCreated]          DATETIME      NOT NULL,
    [CreatedByUserID]      INT           NOT NULL,
    [ResponseOther]        VARCHAR (300) NULL,
	[HistorySubscriptionID] INT	NULL,
    CONSTRAINT [PK_HistoryResponse] PRIMARY KEY CLUSTERED ([HistoryResponseMapID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IX_HistoryResponseMap_HistorySubscriptionID]
    ON [dbo].[HistoryResponseMap]([HistorySubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_HistoryResponseMap_PubSubscriptionID]
    ON [dbo].[HistoryResponseMap]([PubSubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_HistoryResponseMap_SubscriptionID]
    ON [dbo].[HistoryResponseMap]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

