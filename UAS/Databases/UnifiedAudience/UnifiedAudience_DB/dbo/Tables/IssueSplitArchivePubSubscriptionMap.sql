CREATE TABLE [dbo].[IssueSplitArchivePubSubscriptionMap](
	[IssueArchiveSubscriptionId] [int] IDENTITY(1,1) NOT NULL,
	[IssueSplitPubSubscriptionId] [int] NOT NULL,
	[IssueSplitId] [int] NOT NULL,
	RecordMovedFrom int,
 CONSTRAINT [PK_IssueSplitArchivePubSubscriptionMap] PRIMARY KEY CLUSTERED 
(
	[IssueArchiveSubscriptionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
