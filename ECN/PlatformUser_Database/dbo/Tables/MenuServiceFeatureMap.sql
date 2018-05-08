CREATE TABLE [dbo].[MenuServiceFeatureMap] (
    [MenuSFMapID]      INT      IDENTITY (1, 1) NOT NULL,
    [MenuID]           INT      NOT NULL,
    [ServiceFeatureID] INT      NOT NULL,
    [IsActive]         BIT      CONSTRAINT [DF_MenuServiceFeatureMap_IsActive] DEFAULT ((1)) NOT NULL,
    [DateCreated]      DATETIME CONSTRAINT [DF_MenuServiceFeatureMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]      DATETIME NULL,
    [CreatedByUserID]  INT      NOT NULL,
    [UpdatedByUserID]  INT      NULL,
    CONSTRAINT [PK_MenuServiceFeatureMap] PRIMARY KEY CLUSTERED ([MenuSFMapID] ASC),
    CONSTRAINT [FK_MenuServiceFeatureMap_Menu] FOREIGN KEY ([MenuID]) REFERENCES [dbo].[Menu] ([MenuID]),
    CONSTRAINT [FK_MenuServiceFeatureMap_ServiceFeature] FOREIGN KEY ([ServiceFeatureID]) REFERENCES [dbo].[ServiceFeature] ([ServiceFeatureID])
);

