CREATE TABLE [dbo].[IssueSplitFilter](
	 [FilterID] [int] IDENTITY(1,1) NOT NULL,
	 [PubId] [int] NOT NULL,
	 [FilterName] [varchar](512) NOT NULL,
	 DateCreated datetime null,
	 DateUpdated datetime null,
	 CreatedByUserID int null,
	 UpdatedByUserID int null
 CONSTRAINT [PK_IssueSplitFilter] PRIMARY KEY CLUSTERED 
(
	[FilterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

