CREATE TABLE [dbo].[ReportQueue]
(
	[ReportQueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReportID] INT NULL, 
    [Status] VARCHAR(50) NULL, 
    [SendTime] DATETIME NULL, 
    [FinishTime] DATETIME NULL, 
    [FailureReason] VARCHAR(MAX) NULL, 
    [ReportScheduleID] INT NULL
)
