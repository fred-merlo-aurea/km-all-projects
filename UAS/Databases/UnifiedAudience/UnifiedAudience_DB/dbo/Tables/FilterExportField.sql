CREATE TABLE [dbo].[FilterExportField] (
    [FilterExportFieldID] INT           IDENTITY (1, 1) NOT NULL,
    [FilterScheduleID]    INT           NOT NULL,
    [ExportColumn]        VARCHAR (100) NULL,
    [CustomValue]         VARCHAR (100) NULL,
    [IsCustomValue]       BIT           NULL,
    [MappingField]        VARCHAR (100) NULL,
    [IsDescription]       BIT           CONSTRAINT [DF_FilterExportField_IsDescription] DEFAULT ((0)) NULL,
    [FieldCase] VARCHAR(100) NULL, 
    CONSTRAINT [FK_FilterExportField_FilterScheduleID] FOREIGN KEY ([FilterScheduleID]) REFERENCES [dbo].[FilterSchedule] ([FilterScheduleID]), 
    CONSTRAINT [PK_FilterExportField] PRIMARY KEY ([FilterExportFieldID])
);
GO

