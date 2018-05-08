CREATE TABLE [dbo].[History] (
    [HistoryID]             INT      IDENTITY (1, 1) NOT NULL,
    [BatchID]               INT      NOT NULL,
    [BatchCountItem]        INT      NOT NULL,
    [PublisherID]           INT      NOT NULL,
    [PublicationID]         INT      NOT NULL,
    [SubscriberID]          INT      NOT NULL,
    [SubscriptionID]        INT      NOT NULL,
    [HistorySubscriptionID] INT      NULL,
    [HistoryPaidID]         INT      NULL,
    [HistoryPaidBillToID]   INT      NULL,
    [DateCreated]           DATETIME NOT NULL,
    [CreatedByUserID]       INT      NOT NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([HistoryID] ASC) WITH (FILLFACTOR = 80)
);

