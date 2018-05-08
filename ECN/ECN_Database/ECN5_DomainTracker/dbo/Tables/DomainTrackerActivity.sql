CREATE TABLE [dbo].[DomainTrackerActivity] (
    [DomainTrackerActivityID] INT            IDENTITY (1, 1) NOT NULL,
    [DomainTrackerID]         INT            NULL,
    [ProfileID]               INT            NULL,
    [PageURL]                 VARCHAR (8000) NULL,
    [TimeStamp]               DATETIME       NULL,
    [IPAddress]               VARCHAR (50)   NULL,
    [UserAgent]               VARCHAR (8000) NULL,
    [OS]                      VARCHAR (100)  NULL,
    [Browser]                 VARCHAR (100)  NULL,
    [ReferralURL]             VARCHAR (8000) NULL,
    [SourceBlastID]           INT            NULL,
    CONSTRAINT [PK_DomainTrackerActivity] PRIMARY KEY CLUSTERED ([DomainTrackerActivityID] ASC),
    CONSTRAINT [FK_DomainTrackerActivityDomainTrackerID] FOREIGN KEY ([DomainTrackerID]) REFERENCES [dbo].[DomainTracker] ([DomainTrackerID]),
    CONSTRAINT [FK_DomainTrackerActivityProfileID] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[DomainTrackerUserProfile] ([ProfileID])
);








GO
CREATE NONCLUSTERED INDEX [IDX_DomainTrackerActivity_DomainTrackerIDProfileId]
    ON [dbo].[DomainTrackerActivity]([DomainTrackerID] ASC, [ProfileID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_DomainTrackerActivity_DomaintrackerIdTimeStamp]
    ON [dbo].[DomainTrackerActivity]([DomainTrackerID] ASC, [TimeStamp] ASC)
    INCLUDE([PageURL]);


GO
CREATE NONCLUSTERED INDEX [IDX_DomainTrackerActivity_IPAddress]
    ON [dbo].[DomainTrackerActivity]([IPAddress] ASC);

