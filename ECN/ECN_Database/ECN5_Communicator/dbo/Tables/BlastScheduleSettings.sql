CREATE TABLE [dbo].[BlastScheduleSettings] (
    [ScheduleTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [ScheduleID]       INT          NULL,
    [ScheduleType]     VARCHAR (50) NULL,
    [ScheduleTypeCode] CHAR (1)     NULL,
    [ScheduleSettings] VARCHAR (50) NULL,
    [ScheduleTime]     DATETIME     NULL,
    [DateUpdated]      DATETIME     NULL,
    CONSTRAINT [PK_BlastScheduleSettings] PRIMARY KEY CLUSTERED ([ScheduleTypeID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[BlastScheduleSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[BlastScheduleSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastScheduleSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[BlastScheduleSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastScheduleSettings] TO [reader]
    AS [dbo];

