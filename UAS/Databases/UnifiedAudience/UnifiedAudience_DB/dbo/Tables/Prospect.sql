CREATE TABLE [dbo].[Prospect] (
    [ProspectID]         INT      IDENTITY (1, 1) NOT NULL,
    [PublicationID]      INT      NOT NULL,
    [SubscriberID]       INT      NOT NULL,
    [IsProspect]         BIT      CONSTRAINT [DF_PublicationMap_IsProspect] DEFAULT ((0)) NOT NULL,
    [IsVerifiedProspect] BIT      CONSTRAINT [DF_PublicationMap_IsVerifiedProspect] DEFAULT ((0)) NOT NULL,
    [DateCreated]        DATETIME NOT NULL,
    [DateUpdated]        DATETIME NULL,
    [CreatedByUserID]    INT      NOT NULL,
    [UpdatedByUserID]    INT      NULL,
    CONSTRAINT [PK_PublicationMap] PRIMARY KEY CLUSTERED ([PublicationID] ASC, [SubscriberID] ASC) WITH (FILLFACTOR = 80)
);

