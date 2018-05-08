CREATE TABLE [dbo].[DomainTracker] (
    [DomainTrackerID] INT          IDENTITY (1, 1) NOT NULL,
    [TrackerKey]      VARCHAR (50) NULL,
    [Domain]          VARCHAR (50) NULL,
    [CreatedDate]     DATETIME     NULL,
    [CreatedUserID]   INT          NULL,
    [CustomerID]      INT          NULL,
    [GroupID]         INT          NULL,
    [IsDeleted]       BIT          NULL,
    [UpdatedDate]     DATETIME     NULL,
    [UpdatedUserID]   INT          NULL,
    CONSTRAINT [PK_DomainTracker] PRIMARY KEY CLUSTERED ([DomainTrackerID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [IX_DomainTracker_TrackerKey] UNIQUE NONCLUSTERED ([TrackerKey] ASC) WITH (FILLFACTOR = 80)
);

