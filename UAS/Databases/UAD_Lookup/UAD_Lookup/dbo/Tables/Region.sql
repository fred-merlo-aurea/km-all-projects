CREATE TABLE [dbo].[Region] (
    [RegionID]    INT           IDENTITY (1, 1) NOT NULL,
    [CountryID]   INT           NULL,
    [RegionName]  VARCHAR (256) NULL,
    [RegionCode]  VARCHAR (256) NULL,
	[ZipCodeRange] VARCHAR(50) NULL,
	[ZipCodeRangeSortOrder] INT NULL,
	[RegionGroupID] INT NULL,
	[sort_order] int null,
	[country_sort_order] int null
    CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED ([RegionID] ASC)
);

