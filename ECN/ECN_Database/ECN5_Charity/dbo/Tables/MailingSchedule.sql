CREATE TABLE [dbo].[MailingSchedule] (
    [ScheduleID]    INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT          NULL,
    [CalendarCycle] INT          NULL,
    [CalendarDay]   INT          NULL,
    [MailDate]      DATETIME     NULL,
    [PickupDate]    DATETIME     NULL,
    [ZipCode]       VARCHAR (10) NULL,
    [City]          VARCHAR (50) NULL,
    [Route]         VARCHAR (10) NULL,
    [SchedCount]    INT          NULL,
    [DateAdded]     DATETIME     CONSTRAINT [DF_MailingSchedule_DateAdded] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_MailingSchedule] PRIMARY KEY CLUSTERED ([ScheduleID] ASC) WITH (FILLFACTOR = 80)
);

