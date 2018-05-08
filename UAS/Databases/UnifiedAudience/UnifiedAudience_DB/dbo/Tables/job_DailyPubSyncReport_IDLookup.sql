CREATE TABLE [dbo].[job_DailyPubSyncReport_IDLookup] (
    [ResultTypeID] INT           NOT NULL,
    [Errorcode]    VARCHAR (100) NULL,
    [Description]  VARCHAR (255) NULL,
    CONSTRAINT [pk_Job_dailyPubSyncReport_IDLookup] PRIMARY KEY CLUSTERED ([ResultTypeID] ASC)
);

