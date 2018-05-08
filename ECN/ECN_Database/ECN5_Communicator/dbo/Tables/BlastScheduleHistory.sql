CREATE TABLE [dbo].[BlastScheduleHistory] (
    [BlastScheduleHistoryID] INT          IDENTITY (1, 1) NOT NULL,
    [BlastScheduleID]        INT          NOT NULL,
    [SchedTime]              VARCHAR (8)  NULL,
    [SchedStartDate]         VARCHAR (10) NULL,
    [SchedEndDate]           VARCHAR (10) NULL,
    [Period]                 VARCHAR (1)  NULL,
    [CreatedBy]              INT          NULL,
    [CreatedDate]            DATETIME     CONSTRAINT [DF_BlastScheduleHistory_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedBy]              INT          NULL,
    [UpdatedDate]            DATETIME     NULL,
    [Action]                 VARCHAR (50) NULL,
    CONSTRAINT [PK_BlastScheduleHistory] PRIMARY KEY CLUSTERED ([BlastScheduleHistoryID] ASC) WITH (FILLFACTOR = 80)
);

