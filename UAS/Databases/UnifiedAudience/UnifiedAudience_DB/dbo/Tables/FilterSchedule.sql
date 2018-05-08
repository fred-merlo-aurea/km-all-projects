﻿CREATE TABLE [dbo].[FilterSchedule] (
    [FilterScheduleID]         INT           IDENTITY (1, 1) NOT NULL,
    [FilterID]                 INT           NOT NULL,
    [ExportTypeID]             INT           NOT NULL,
    [IsRecurring]              BIT           NULL,
    [RecurrenceTypeID]         INT           NULL,
    [StartDate]                DATE          NULL,
    [StartTime]                VARCHAR (8)   NULL,
    [EndDate]                  DATE          NULL,
    [RunSunday]                BIT           NULL,
    [RunMonday]                BIT           NULL,
    [RunTuesday]               BIT           NULL,
    [RunWednesday]             BIT           NULL,
    [RunThursday]              BIT           NULL,
    [RunFriday]                BIT           NULL,
    [RunSaturday]              BIT           NULL,
    [MonthScheduleDay]         INT           NULL,
    [MonthLastDay]             BIT           NULL,
    [CreatedDate]              DATETIME      NULL,
    [CreatedBy]                INT           NULL,
    [UpdatedDate]              DATETIME      NULL,
    [UpdatedBy]                INT           NULL,
    [EmailNotification]        VARCHAR (500) NULL,
    [Server]                   VARCHAR (50)  NULL,
    [UserName]                 VARCHAR (50)  NULL,
    [Password]                 VARCHAR (50)  NULL,
    [Folder]                   VARCHAR (50)  NULL,
    [ExportFormat]             VARCHAR (50)  NULL,
    [FileName]                 VARCHAR (50)  NULL,
    [IsDeleted]                BIT           CONSTRAINT [DF_FilterSchedule_IsDelete] DEFAULT ((0)) NULL,
    [CustomerID]               INT           NULL,
    [FilterGroupID_Selected]   VARCHAR (500) NULL,
    [FilterGroupID_Suppressed] VARCHAR (500) NULL,
    [FolderID]                 INT           NULL,
    [GroupID]                  INT           NULL,
    [Operation]                VARCHAR (50)  NULL,
    [ShowHeader]               BIT           DEFAULT ((0)) NULL,
    [AppendDateTimeToFileName] BIT           CONSTRAINT [DF_FilterSchedule_AppendDateTimeToFileName] DEFAULT ((0)) NULL,
    [ExportName] VARCHAR(50) NULL, 
    [ExportNotes] VARCHAR(250) NULL, 
    [FilterSegmentationID] INT NULL, 
    [SelectedOperation] VARCHAR(50) NULL, 
    [SuppressedOperation] VARCHAR(50) NULL, 
	[FileNameFormat] VARCHAR(50) NULL,
    CONSTRAINT [PK_FilterSchedule] PRIMARY KEY CLUSTERED ([FilterScheduleID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FilterSchedule_ExportType] FOREIGN KEY ([ExportTypeID]) REFERENCES [dbo].[ExportType] ([ExportTypeID]),
    CONSTRAINT [FK_FilterSchedule_Filters] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filters] ([FilterID]),
	CONSTRAINT [FK_FilterSchedule_FilterSegmentation] FOREIGN KEY ([FilterSegmentationID]) REFERENCES [dbo].[FilterSegmentation] ([FilterSegmentationID]),
    CONSTRAINT [FK_FilterSchedule_RecurrenceType] FOREIGN KEY ([RecurrenceTypeID]) REFERENCES [dbo].[RecurrenceType] ([RecurrenceTypeID])
);
GO

