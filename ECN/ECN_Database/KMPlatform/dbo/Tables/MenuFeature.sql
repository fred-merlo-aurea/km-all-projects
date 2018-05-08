CREATE TABLE [dbo].[MenuFeature] (
    [MenuFeatureID]   INT           IDENTITY (1, 1) NOT NULL,
    [FeatureName]     NVARCHAR (50) NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_MenuFeatu_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    CONSTRAINT [PK_MenuFeature] PRIMARY KEY CLUSTERED ([MenuFeatureID] ASC) WITH (FILLFACTOR = 80)
);

