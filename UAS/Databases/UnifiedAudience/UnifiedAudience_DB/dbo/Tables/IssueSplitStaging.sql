CREATE TABLE [dbo].[IssueSplitStaging](
	[IssueSplitId] [int] IDENTITY(1,1) NOT NULL,
	[IssueId] [int] NOT NULL,
	[IssueSplitCode] [varchar](250) NOT NULL,
	[IssueSplitName] [varchar](250) NOT NULL,
	[IssueSplitCount] [int] NOT NULL,
	[FilterId] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedByUserID] [int] NOT NULL,
	[UpdatedByUserID] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[KeyCode] [varchar](250) NULL,
	[IssueSplitRecords] [int] NULL,
	[IssueSplitDescription] [varchar](250) NULL,
 CONSTRAINT [PK_IssueSplitStaging] PRIMARY KEY CLUSTERED 
(
	[IssueSplitId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IssueSplitStaging] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[IssueSplitStaging] ADD  DEFAULT ((0)) FOR [IssueSplitRecords]
GO

