CREATE TABLE [dbo].[APILogging]
(
	[APILogID]		int					IDENTITY(1,1) not null,
	[AccessKey]		uniqueidentifier	null,
	[APIMethod]		varchar(255)		null,
	[Input]			text				null,
	[StartTime]		datetime			null,
	[LogID]			int					null,
	[EndTime]		datetime			null
	CONSTRAINT [PK_APILogging_APILogID] PRIMARY KEY CLUSTERED ([APILogID] ASC) WITH (FILLFACTOR = 90)
);
