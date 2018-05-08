CREATE TABLE [dbo].[PageWatch] (
    [PageWatchID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [URL]           VARCHAR (300) NOT NULL,
    [AdminUserID]   INT           NOT NULL,
    [GroupID]       INT           NOT NULL,
    [LayoutID]      INT           NOT NULL,
    [FrequencyType] VARCHAR (10)  NOT NULL,
    [FrequencyNo]   INT           NOT NULL,
    [AddedBy]       INT           NOT NULL,
    [DateAdded]     DATETIME      NOT NULL,
    [UpdatedBy]     INT           NULL,
    [DateUpdated]   DATETIME      NULL,
    [ScheduleTime]  DATETIME      NULL,
    [IsActive]      BIT           CONSTRAINT [DF_PageWatch_IsActive] DEFAULT ((1)) NOT NULL,
    [CustomerID]    INT           NOT NULL,
    CONSTRAINT [PK_PageWatch] PRIMARY KEY CLUSTERED ([PageWatchID] ASC) WITH (FILLFACTOR = 80)
);

