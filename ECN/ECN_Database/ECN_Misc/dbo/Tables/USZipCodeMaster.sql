CREATE TABLE [dbo].[USZipCodeMaster] (
    [ZipCodeID]  INT           IDENTITY (1, 1) NOT NULL,
    [ZIPCode]    VARCHAR (255) NULL,
    [ZIPType]    VARCHAR (255) NULL,
    [CityName]   VARCHAR (255) NULL,
    [CityType]   VARCHAR (255) NULL,
    [CountyName] VARCHAR (255) NULL,
    [CountyFIPS] VARCHAR (255) NULL,
    [StateName]  VARCHAR (255) NULL,
    [StateAbbr]  VARCHAR (255) NULL,
    [StateFIPS]  VARCHAR (255) NULL,
    [MSACode]    VARCHAR (255) NULL,
    [AreaCode]   VARCHAR (255) NULL,
    [TimeZone]   VARCHAR (255) NULL,
    [UTC]        VARCHAR (255) NULL,
    [DST]        VARCHAR (255) NULL,
    [Latitude]   VARCHAR (255) NULL,
    [Longitude]  VARCHAR (255) NULL,
    CONSTRAINT [PK_USZipCodeMaster] PRIMARY KEY CLUSTERED ([ZipCodeID] ASC) WITH (FILLFACTOR = 80)
);

