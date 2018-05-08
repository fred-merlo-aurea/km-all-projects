CREATE TABLE [dbo].[job_DailyPubSyncReport] (
    [JobID]        INT      IDENTITY (1, 1) NOT NULL,
    [PubID]        INT      NOT NULL,
    [GroupID]      INT      NOT NULL,
    [Counts]       INT      NOT NULL,
    [isError]      BIT      NOT NULL,
    [HasEmailID]   INT      NOT NULL,
    [RunDate]      DATETIME NOT NULL,
    [ResultTypeID] INT      NOT NULL,
    CONSTRAINT [pk_Job_dailyPubSyncReport] PRIMARY KEY CLUSTERED ([JobID] ASC)
);

