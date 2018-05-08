CREATE TABLE [dbo].[BlastScheduleDaysHistory] (
    [BlastScheduleDaysHistoryID] INT IDENTITY (1, 1) NOT NULL,
    [BlastScheduleDaysID]        INT NOT NULL,
    [BlastScheduleID]            INT NOT NULL,
    [DayToSend]                  INT NULL,
    [IsAmount]                   BIT NULL,
    [Total]                      INT NULL,
    [Weeks]                      INT NULL,
    [BlastScheduleHistoryID]     INT NULL,
    CONSTRAINT [PK_BlastScheduleDaysHistory] PRIMARY KEY CLUSTERED ([BlastScheduleDaysHistoryID] ASC) WITH (FILLFACTOR = 80)
);

