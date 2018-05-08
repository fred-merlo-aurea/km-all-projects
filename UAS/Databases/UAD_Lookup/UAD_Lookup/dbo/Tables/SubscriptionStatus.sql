CREATE TABLE [dbo].[SubscriptionStatus] (
    [SubscriptionStatusID] INT          IDENTITY (1, 1) NOT NULL,
    [StatusName]           VARCHAR (50) NOT NULL,
    [StatusCode]           VARCHAR (50) NOT NULL,
    [Color]                VARCHAR (50) NOT NULL,
    [Icon]                 VARCHAR (50) NOT NULL,
    [IsActive]             BIT          NOT NULL,
    [DateCreated]          DATETIME     NOT NULL,
    [DateUpdated]          DATETIME     NULL,
    [CreatedByUserID]      INT          NOT NULL,
    [UpdatedByUserID]      INT          NULL,
    CONSTRAINT [PK_SubscriptionStatus] PRIMARY KEY CLUSTERED ([SubscriptionStatusID] ASC) WITH (FILLFACTOR = 80)
);

