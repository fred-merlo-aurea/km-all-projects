CREATE TABLE [dbo].[Notification](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[NotificationName] [varchar](100) NULL,
	[IsImage] [bit] NULL,
	[ImageURL] [varchar](255) NULL,
	[NotificationText] [text] NULL,
	[StartDate] [varchar](10) NULL,
	[StartTime] [varchar](10) NULL,
	[EndDate] [varchar](10) NULL,
	[EndTime] [varchar](10) NULL,
	[CreatedUserID] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedUserID] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsDeleted] [int] NULL,
	[BackGroundColor] [varchar](20) NULL,
	[CloseButtonColor] [varchar](20) NULL
)