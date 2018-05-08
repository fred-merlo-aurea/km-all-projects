CREATE TABLE [dbo].[SocialMediaErrorCodes](
	[id_num] [int] IDENTITY(1,1) NOT NULL,
	[mediaType] [int] NULL,
	[errorCodeRepost] [int] NULL,
	[errorCodeNoRepost] [int] NULL,
	[errorMsg] [varchar](100) NULL,
	[friendlyMsg] [varchar](100) NULL
) ON [PRIMARY]

GO

