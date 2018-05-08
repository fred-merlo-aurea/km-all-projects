CREATE TABLE [dbo].[ServiceFeatureAccessMap] (
    [ServiceFeatureAccessMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ServiceFeatureID]          INT      NOT NULL,
    [AccessID]                  INT      NOT NULL,
    [IsEnabled]                 BIT      CONSTRAINT [DF_ServiceFeatureAccessMap_IsEnabled] DEFAULT (('true')) NOT NULL,
    [DateCreated]               DATETIME CONSTRAINT [DF_ServiceFeatureAccessMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]               DATETIME NULL,
    [CreatedByUserID]           INT      NOT NULL,
    [UpdatedByUserID]           INT      NULL,
    [Description] VARCHAR(500) NULL, 
    CONSTRAINT [PK_ServiceFeatureAccessMap] PRIMARY KEY CLUSTERED ([ServiceFeatureAccessMapID] ASC),
    CONSTRAINT [FK_ServiceFeatureAccessMap_Access] FOREIGN KEY ([AccessID]) REFERENCES [dbo].[Access] ([AccessID]),
    CONSTRAINT [FK_ServiceFeatureAccessMap_ServiceFeature] FOREIGN KEY ([ServiceFeatureID]) REFERENCES [dbo].[ServiceFeature] ([ServiceFeatureID])
);

