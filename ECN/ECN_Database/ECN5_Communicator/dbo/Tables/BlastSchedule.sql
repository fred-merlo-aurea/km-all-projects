CREATE TABLE [dbo].[BlastSchedule] (
    [BlastScheduleID] INT          IDENTITY (1, 1) NOT NULL,
    [SchedTime]       VARCHAR (8)  NULL,
    [SchedStartDate]  VARCHAR (10) NULL,
    [SchedEndDate]    VARCHAR (10) NULL,
    [Period]          VARCHAR (1)  NULL,
    [CreatedBy]       INT          NULL,
    [CreatedDate]     DATETIME     CONSTRAINT [DF_BlastSchedule_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedBy]       INT          NULL,
    [UpdatedDate]     DATETIME     NULL,
    [SplitType]       VARCHAR (1)  NULL,
    CONSTRAINT [PK_BlastSchedule] PRIMARY KEY CLUSTERED ([BlastScheduleID] ASC) WITH (FILLFACTOR = 80)
);



