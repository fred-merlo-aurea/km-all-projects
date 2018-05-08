CREATE TABLE [dbo].[Log_MergeSubscriber]
(
	[Log_MergeSubscriberID] INT IDENTITY(1,1) NOT NULL,
	[SubscriptionIDToKeep] INT NOT NULL,
	[SubscriptionIDToRemove] INT NOT NULL,
	[PubSubscriptionIDToKeep] VARCHAR(max) NOT NULL,
	[PubSubscriptionIDToRemove] VARCHAR(max) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[CreatedByUserID] INT NOT NULL,
	CONSTRAINT [PK_Log_MergeSubscriber] PRIMARY KEY CLUSTERED ([Log_MergeSubscriberID] ASC)
);
