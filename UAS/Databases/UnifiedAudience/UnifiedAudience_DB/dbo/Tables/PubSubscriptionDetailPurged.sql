
CREATE TABLE [dbo].[PubSubscriptionDetailPurged](
	[PubSubscriptionDetailPurgedID] [int] IDENTITY(1,1) NOT NULL,
	[PurgedDate] [datetime] NOT NULL,
	[PubSubscriptionDetailID] [int] NOT NULL,
	[PubSubscriptionID] [int] NULL,
	[SubscriptionID] [int] NULL,
	[CodesheetID] [int] NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedByUserID] [int] NULL,
	[UpdatedByUserID] [int] NULL,
	[ResponseOther] [varchar](256) NULL,
 CONSTRAINT [PK_PubSubscriptionDetailPurged] PRIMARY KEY CLUSTERED 
(
	[PubSubscriptionDetailPurgedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PubSubscriptionDetailPurged] ADD  CONSTRAINT [DF_PubSubscriptionDetailPurged_PurgedDate]  DEFAULT (getdate()) FOR [PurgedDate]
GO

