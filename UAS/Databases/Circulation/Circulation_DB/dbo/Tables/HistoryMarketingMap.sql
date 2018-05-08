CREATE TABLE [dbo].[HistoryMarketingMap] (
    [HistoryMarketingMapID] INT      IDENTITY (1, 1) NOT NULL,
    [MarketingID]           INT      NOT NULL,
    [SubscriberID]          INT      NOT NULL,
    [PublicationID]         INT      NOT NULL,
    [IsActive]              BIT      NOT NULL,
    [DateCreated]           DATETIME NOT NULL,
    [CreatedByUserID]       INT      NOT NULL,
    CONSTRAINT [PK_HistoryMarketingMap] PRIMARY KEY CLUSTERED ([HistoryMarketingMapID] ASC) WITH (FILLFACTOR = 80)
);

