CREATE TABLE [dbo].[MenuMenuFeatureMap] (
    [MenuMenuFeatureMapID] INT      IDENTITY (1, 1) NOT NULL,
    [MenuID]               INT      NOT NULL,
    [MenuFeatureID]        INT      NOT NULL,
    [HasAccess]            BIT      NOT NULL,
    [DateCreated]          DATETIME NOT NULL,
    [DateUpdated]          DATETIME NULL,
    [CreatedByUserID]      INT      NOT NULL,
    [UpdatedByUserID]      INT      NULL,
    CONSTRAINT [PK_MenuFeatureMap] PRIMARY KEY CLUSTERED ([MenuID] ASC, [MenuFeatureID] ASC),
    CONSTRAINT [FK_MenuMenuFeatureMap_Menu] FOREIGN KEY ([MenuID]) REFERENCES [dbo].[Menu] ([MenuID]),
    CONSTRAINT [FK_MenuMenuFeatureMap_MenuFeature] FOREIGN KEY ([MenuFeatureID]) REFERENCES [dbo].[MenuFeature] ([MenuFeatureID])
);

