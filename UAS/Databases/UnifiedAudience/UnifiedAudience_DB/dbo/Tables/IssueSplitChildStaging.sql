CREATE TABLE [dbo].[IssueSplitChildStaging](
	[IssueSplitChildId] [int] IDENTITY(1,1) NOT NULL,
	[IssueSplitChildCount] [int] NOT NULL,
	[IssueSplitChildName] varchar(500) NULL ,
	[IssueSplitChildDesc] varchar(500) NULL ,
	[IssueSplitParentId] [int] NOT NULL,
	CONSTRAINT [PK_IssueSplitChildStaging] PRIMARY KEY CLUSTERED (
	[IssueSplitChildId] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]