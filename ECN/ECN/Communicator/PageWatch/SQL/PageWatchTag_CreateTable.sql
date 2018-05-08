USE [ecn5_communicator]
GO

/****** Object:  Table [dbo].[PageWatchTag]    Script Date: 07/18/2011 13:21:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PageWatchTag](
	[PageWatchTagID] [int] IDENTITY(1,1) NOT NULL,
	[PageWatchID] [int] NOT NULL,
	[Name] [varchar](100) NULL,
	[WatchTag] [varchar](300) NOT NULL,
	[PreviousHTML] [text] NULL,
	[AddedBy] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[DateUpdated] [datetime] NULL,
	[IsChanged] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_PageWatchTag] PRIMARY KEY CLUSTERED 
(
	[PageWatchTagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PageWatchTag]  WITH CHECK ADD  CONSTRAINT [FK_PageWatchTag_PageWatch] FOREIGN KEY([PageWatchID])
REFERENCES [dbo].[PageWatch] ([PageWatchID])
GO

ALTER TABLE [dbo].[PageWatchTag] CHECK CONSTRAINT [FK_PageWatchTag_PageWatch]
GO

ALTER TABLE [dbo].[PageWatchTag] ADD  CONSTRAINT [DF_PageWatchTag_IsChanged]  DEFAULT ((0)) FOR [IsChanged]
GO

ALTER TABLE [dbo].[PageWatchTag] ADD  CONSTRAINT [DF_PageWatchTag_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO


