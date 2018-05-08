CREATE TABLE [dbo].[IssueSplitFilterDetails](
	[FilterDetailID] [int] IDENTITY(1,1) NOT NULL,
	[FilterID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Values] [varchar](max) NULL,
	[SearchCondition] [varchar](50) NULL,
	[Group] [varchar](100) NULL,
	[Text] varchar(max),
 CONSTRAINT [PK_IssueSplitFilterDetails] PRIMARY KEY CLUSTERED 
(
	[FilterDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

