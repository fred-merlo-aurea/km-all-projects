CREATE TABLE [dbo].[FilterExportField]
(
	[FilterExportFieldId] INT           IDENTITY (1, 1) NOT NULL,
    [FilterScheduleId]    INT           NOT NULL,
    [ExportColumn]        VARCHAR (100) NULL,
	[DateCreated]                  DATETIME       NOT NULL,
    [DateUpdated]                  DATETIME       NULL,
    [CreatedByUserID]              INT            NOT NULL,
    [UpdatedByUserID]              INT       NULL,
    CONSTRAINT [FK_FilterExportField_FilterScheduleID] FOREIGN KEY ([FilterScheduleID]) REFERENCES [dbo].[FilterSchedule] ([FilterScheduleID])
)
