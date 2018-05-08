CREATE TABLE [ApplicationLog](
	[ApplicationLogId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[SeverityCodeId] [int] NOT NULL,
	[SourceMethod] [varchar](250) NULL,
	[Exception] [varchar](max) NOT NULL,
	[LogNote] [varchar](max) NULL,
	[IsBug] [bit] NULL,
	[IsUserSubmitted] [bit] NULL,
	[ClientId] [int] NULL,
	[SubmittedBy] [varchar](250) NULL,
	[SubmittedByEmail] [varchar](100) NULL,
	[IsFixed] [bit] NULL,
	[FixedDate] [date] NULL,
	[FixedTime] [time](7) NULL,
	[FixedBy] [varchar](50) NULL,
	[FixedNote] [varchar](750) NULL,
	[LogAddedDate] [date] NOT NULL DEFAULT GETDATE(),
	[LogAddedTime] [time](7) NOT NULL DEFAULT GETDATE(),
	[LogUpdatedDate] [date] NULL,
	[LogUpdatedTime] [time](7) NULL,
	[NotificationSent] [bit] NOT NULL DEFAULT 'false',
 CONSTRAINT [PK_ApplicationLog] PRIMARY KEY CLUSTERED 
(
	[ApplicationLogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
