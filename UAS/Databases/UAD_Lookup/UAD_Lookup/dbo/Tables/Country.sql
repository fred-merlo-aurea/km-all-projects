CREATE TABLE [dbo].[Country] (
    [CountryID]   INT           NOT NULL,
	[FullName] VARCHAR (256) NULL,
    [ShortName] VARCHAR (256) NULL,
    [PhonePrefix] INT           NULL,
	[Continent]   VARCHAR (25) NULL,
	[ContinentCode]   CHAR (2) NULL,
    [Area]        VARCHAR (256) NULL,
	[Alpha2] CHAR (2) NULL,
	[Alpha3] CHAR (3) NULL,
	[ISOCountryCode] CHAR(3) NULL,
	[SortOrder] INT	NULL,
	[BpaArea] VARCHAR (256) NULL,
	[BpaName] VARCHAR (256) NULL,
	[BpaAreaOrder] int null,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryID] ASC)
);



