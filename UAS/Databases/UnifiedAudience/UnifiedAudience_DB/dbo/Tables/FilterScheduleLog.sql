CREATE TABLE [dbo].[FilterScheduleLog]
(
	FilterScheduleLogID int IDENTITY(1,1) NOT NULL,
	FilterScheduleID int NULL,
	StartDate date NULL,
	StartTime varchar(8) NULL,
	[FileName] varchar(50) NULL,
	DownloadCount int NULL,
    CONSTRAINT [PK_FilterScheduleLog] PRIMARY KEY CLUSTERED ([FilterScheduleLogID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FilterScheduleLog_FilterSchedule] FOREIGN KEY ([FilterScheduleID]) REFERENCES [dbo].[FilterSchedule] ([FilterScheduleID])
)
