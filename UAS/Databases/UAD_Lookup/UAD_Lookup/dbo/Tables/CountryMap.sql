CREATE TABLE [dbo].[CountryMap] (
    [CountryID]    INT           NOT NULL,
    [CountryDirty] VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([CountryDirty] ASC),
);

