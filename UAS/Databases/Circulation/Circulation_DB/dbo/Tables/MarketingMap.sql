CREATE TABLE [dbo].[MarketingMap] (
    [MarketingID]     INT      NOT NULL,
    [SubscriberID]    INT      NOT NULL,
    [PublicationID]   INT      NOT NULL,
    [IsActive]        BIT      CONSTRAINT [DF_MarketingMap_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]     DATETIME NOT NULL,
    [DateUpdated]     DATETIME NULL,
    [CreatedByUserID] INT      NOT NULL,
    [UpdatedByUserID] INT      NULL,
    CONSTRAINT [PK_MarketingMap] PRIMARY KEY CLUSTERED ([MarketingID] ASC, [SubscriberID] ASC, [PublicationID] ASC) WITH (FILLFACTOR = 80)
);

