CREATE TABLE [dbo].[CrossTabReport]
(
	[CrossTabReportID] [int] IDENTITY(1,1) NOT NULL,
	[CrossTabReportName] [varchar](50) NULL,
	[Row] VARCHAR(50) NULL,
	[Column] VARCHAR(50) NULL,
	[ViewType] [varchar](50) NULL,
	[PubID] [int] NULL,
	[BrandID] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUserID] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUserID] [int] NULL,
	[IsDeleted] [bit] NULL DEFAULT ((0)),
	CONSTRAINT [PK_CrossTabReport] PRIMARY KEY CLUSTERED ([CrossTabReportID] ASC),
	CONSTRAINT [FK_CrossTabReport_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
)
