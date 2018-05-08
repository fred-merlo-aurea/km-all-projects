CREATE TABLE [dbo].[ZipCodes] (
    [ZipCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [StateID]   INT          NULL,
    [ZipCode]   VARCHAR (5)  NULL,
    [CityName]  VARCHAR (50) NULL,
    [StateCode] VARCHAR (2)  NULL,
    CONSTRAINT [PK_ZipCodes] PRIMARY KEY CLUSTERED ([ZipCodeID] ASC) WITH (FILLFACTOR = 80)
);

