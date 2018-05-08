CREATE TABLE [dbo].[SubscriberTopicActivity] (
    [TopicActivityID]   INT           IDENTITY (1, 1) NOT NULL,
    [PubSubscriptionID] INT           NULL,
    [TopicCode]         VARCHAR (100) NULL,
    [ActivityDate]      DATE          NULL,
    [SubscriptionID]    INT           NULL,
    CONSTRAINT [PK_SubscriberTopicActivity] PRIMARY KEY CLUSTERED ([TopicActivityID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriberTopicActivity_PubSubscriptions] FOREIGN KEY ([PubSubscriptionID]) REFERENCES [dbo].[PubSubscriptions] ([PubSubscriptionID])
);
GO

CREATE NONCLUSTERED INDEX [IX_SubscriberTopicActivity_PubSubscriptionID]
    ON [dbo].[SubscriberTopicActivity]([PubSubscriptionID] ASC);
GO
