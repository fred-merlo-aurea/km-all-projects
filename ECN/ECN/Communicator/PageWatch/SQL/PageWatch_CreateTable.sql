USE [ecn5_communicator]
GO

/****** Object:  Table [dbo].[PageWatch]    Script Date: 07/19/2011 09:13:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PageWatch](
	[PageWatchID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[URL] [varchar](300) NOT NULL,
	[AdminUserID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[LayoutID] [int] NOT NULL,
	[FrequencyType] [varchar](10) NOT NULL,
	[FrequencyNo] [int] NOT NULL,
	[AddedBy] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[DateUpdated] [datetime] NULL,
	[ScheduleTime] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CustomerID] [int] NOT NULL,
 CONSTRAINT [PK_PageWatch] PRIMARY KEY CLUSTERED 
(
	[PageWatchID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PageWatch] ADD  CONSTRAINT [DF_PageWatch_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO


