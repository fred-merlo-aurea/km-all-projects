CREATE TABLE [dbo].[FilterScheduleIntegration]
(
	[FilterScheduleIntegrationID] INT IDENTITY (1, 1) NOT NULL, 
	[FilterScheduleID] [int] NOT NULL,
    [IntegrationParamName] VARCHAR(50) NULL, 
    [IntegrationParamValue] VARCHAR(50) NULL,
	CONSTRAINT [PK_FilterScheduleIntergration] PRIMARY KEY CLUSTERED ([FilterScheduleIntegrationID] ASC) WITH (FILLFACTOR = 90),
	CONSTRAINT [FK_FilterScheduleIntegration_FilterSchedule] FOREIGN KEY ([FilterScheduleID]) REFERENCES [dbo].[FilterSchedule] ([FilterScheduleID])
)
