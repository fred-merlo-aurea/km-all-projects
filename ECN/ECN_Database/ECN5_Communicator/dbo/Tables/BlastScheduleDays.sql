CREATE TABLE [dbo].[BlastScheduleDays] (
    [BlastScheduleDaysID] INT IDENTITY (1, 1) NOT NULL,
    [BlastScheduleID]     INT NOT NULL,
    [DayToSend]           INT NULL,
    [IsAmount]            BIT NULL,
    [Total]               INT NULL,
    [Weeks]               INT NULL,
    CONSTRAINT [PK_BlastScheduleDays] PRIMARY KEY CLUSTERED ([BlastScheduleDaysID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_BlastScheduleDays_BlastSchedule] FOREIGN KEY ([BlastScheduleID]) REFERENCES [dbo].[BlastSchedule] ([BlastScheduleID])
);

