CREATE TABLE [dbo].[IssueSplitChildStagingPubSub](
	[IssueSplitChildPubSubId] [int] IDENTITY(1,1) NOT NULL,
	[IssueSplitChildId] [int] NOT NULL,
	[PubSubscriptionID] [int] NOT NULL,
	CONSTRAINT [PK_IssueSplitChildStagingPubSub] PRIMARY KEY CLUSTERED (
	[IssueSplitChildPubSubId] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]


