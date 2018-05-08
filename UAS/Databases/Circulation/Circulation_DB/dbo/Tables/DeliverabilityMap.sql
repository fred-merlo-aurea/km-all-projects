CREATE TABLE [dbo].[DeliverabilityMap] (
    [DeliverabilityID] INT      NOT NULL,
    [PublicationID]    INT      NOT NULL,
    [IsAvailable]      BIT      CONSTRAINT [DF_DeliverabilityMap_IsAvailable] DEFAULT ((0)) NOT NULL,
    [DateCreated]      DATETIME NOT NULL,
    [DateUpdated]      DATETIME NULL,
    [CreatedByUserID]  INT      NOT NULL,
    [UpdatedByUserID]  INT      NULL,
    CONSTRAINT [PK_DeliverabilityMap] PRIMARY KEY CLUSTERED ([DeliverabilityID] ASC, [PublicationID] ASC) WITH (FILLFACTOR = 80)
);

