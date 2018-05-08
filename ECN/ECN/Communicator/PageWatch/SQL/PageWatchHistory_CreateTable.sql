USE [ecn5_communicator]
GO

/****** Object:  Table [dbo].[PageWatchHistory]    Script Date: 07/26/2011 11:23:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PageWatchHistory](
	[PageWatchHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[PageWatchTagID] [int] NOT NULL,
	[PreviousHTML] [text] NOT NULL,
	[CurrentHTML] [text] NOT NULL,
	[StatusCode] [varchar](50) NOT NULL,
	[AddedBy] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[BlastID] [int] NULL,
 CONSTRAINT [PK_PageWatchHistory] PRIMARY KEY CLUSTERED 
(
	[PageWatchHistoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PageWatchHistory]  WITH CHECK ADD  CONSTRAINT [FK_PageWatchHistory_PageWatchTag] FOREIGN KEY([PageWatchTagID])
REFERENCES [dbo].[PageWatchTag] ([PageWatchTagID])
GO

ALTER TABLE [dbo].[PageWatchHistory] CHECK CONSTRAINT [FK_PageWatchHistory_PageWatchTag]
GO


