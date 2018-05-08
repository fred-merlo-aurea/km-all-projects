CREATE TABLE [dbo].[SubscriberAddKillDetail] (
    [AddKillDetailID] INT IDENTITY (1, 1) NOT NULL,
    [AddKillID]       INT NOT NULL,
    [SubscriptionID]  INT NULL,
    CONSTRAINT [PK_SubscriberAddKillDetail] PRIMARY KEY CLUSTERED ([AddKillDetailID] ASC),
    CONSTRAINT [FK_SubscriberAddKillDetail_SubscriberAddKill] FOREIGN KEY ([AddKillID]) REFERENCES [dbo].[SubscriberAddKill] ([AddKillID]),
    CONSTRAINT [FK_SubscriberAddKillDetail_Subscription] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscription] ([SubscriptionID])
);

