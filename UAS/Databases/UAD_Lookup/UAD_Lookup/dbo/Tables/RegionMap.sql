CREATE TABLE [dbo].[RegionMap] (
    [RegionID]    INT           NOT NULL,
    [RegionDirty] VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_RegionMap] PRIMARY KEY CLUSTERED ([RegionID] ASC, [RegionDirty] ASC)
);

