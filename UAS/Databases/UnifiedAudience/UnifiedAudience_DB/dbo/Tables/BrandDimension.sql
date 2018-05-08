CREATE TABLE [dbo].[BrandDimension] (
    [BrandDimensionID] INT IDENTITY (1, 1) NOT NULL,
    [BrandID]          INT NULL,
    [MasterGroupID]    INT NULL,
    [EnableSearching]  BIT NULL,
    [IsRequired]       BIT NULL,
    [IsMultipleValue]  BIT NULL,
    [SortOrder]        INT NULL,
    CONSTRAINT [PK_BrandDimension] PRIMARY KEY CLUSTERED ([BrandDimensionID] ASC)
);

