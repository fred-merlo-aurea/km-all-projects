CREATE TABLE [dbo].[NEBook_Regions] (
    [RegionID]          INT          IDENTITY (1, 1) NOT NULL,
    [RegionName]        VARCHAR (50) NOT NULL,
    [UserID]            INT          NULL,
    [RegionManagerName] VARCHAR (50) NULL,
    CONSTRAINT [PK_NEBook_Regions] PRIMARY KEY CLUSTERED ([RegionID] ASC) WITH (FILLFACTOR = 80)
);

