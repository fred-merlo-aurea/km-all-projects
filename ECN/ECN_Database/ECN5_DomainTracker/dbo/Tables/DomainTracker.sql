CREATE TABLE [dbo].[DomainTracker] (
    [DomainTrackerID] INT          IDENTITY (1, 1) NOT NULL,
    [TrackerKey]      VARCHAR (50) NULL,
    [Domain]          VARCHAR (50) NULL,
    [CustomerID]      INT          NULL,
    [CreatedUserID]   INT          NULL,
    [CreatedDate]     DATETIME     NULL,
    [UpdatedUserID]   INT          NULL,
    [UpdatedDate]     DATETIME     NULL,
    [IsDeleted]       BIT          NULL,
	[BaseChannelID]   int		   NOT NULL default -1,
    CONSTRAINT [PK_DomainTracker] PRIMARY KEY CLUSTERED ([DomainTrackerID] ASC) WITH (FILLFACTOR = 80)
);

