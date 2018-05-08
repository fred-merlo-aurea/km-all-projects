CREATE TABLE [dbo].[SubscriberAddKill] (
    [AddKillID]     INT          IDENTITY (1, 1) NOT NULL,
    [PublicationID] INT          NULL,
    [Count]         INT          NULL,
    [AddKillCount]  INT          NULL,
    [Type]          VARCHAR (50) NULL,
	[IsActive]		BIT			NOT NULL Default 1,
    [CreatedByUserID] INT          NULL,
    [DateCreated]   DATETIME     NULL,
    [UpdatedByUserID] INT          NULL,
    [DateUpdated]   DATETIME     NULL,
    CONSTRAINT [PK_SubscriberAddKill] PRIMARY KEY CLUSTERED ([AddKillID] ASC) WITH (FILLFACTOR = 90)
);

