CREATE TABLE [dbo].[NEBook_SuperRegions] (
    [SuperRegionID]          INT          IDENTITY (1, 1) NOT NULL,
    [SuperRegionName]        VARCHAR (50) NOT NULL,
    [UserID]                 INT          NOT NULL,
    [SuperRegionManagerName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NEBook_SuperRegions] PRIMARY KEY CLUSTERED ([SuperRegionID] ASC) WITH (FILLFACTOR = 80)
);

