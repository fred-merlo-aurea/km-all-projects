CREATE TABLE [dbo].[SubscriberAddKillDetail] (
    [AddKillDetailID]   INT IDENTITY (1, 1) NOT NULL,
    [AddKillID]         INT NOT NULL,
    [PubSubscriptionID] INT NULL,
    [PubCategoryID]     INT NULL,
    [PubTransactionID]  INT NULL,
    CONSTRAINT [PK_SubscriberAddKillDetail] PRIMARY KEY CLUSTERED ([AddKillDetailID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriberAddKillDetail_SubscriberAddKill] FOREIGN KEY ([AddKillID]) REFERENCES [dbo].[SubscriberAddKill] ([AddKillID]),
    CONSTRAINT [FK_SubscriberAddKillDetail_Subscription] FOREIGN KEY ([PubSubscriptionID]) REFERENCES [dbo].[PubSubscriptions] ([PubSubscriptionID])
);