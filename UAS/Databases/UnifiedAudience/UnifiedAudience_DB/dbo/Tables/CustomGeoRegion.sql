CREATE TABLE [dbo].[CustomGeoRegion]
(
	[CustomGeoRegionId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PubID] int not null, 
    [CustomAreaID] INT NOT NULL, 
	[CountryID] int Not Null,
	[AreaOrder] int Not Null,
    CONSTRAINT [FK_CustomGeoRegion_CustomArea] FOREIGN KEY (CustomAreaID) REFERENCES [dbo].[CustomArea]([CustomAreaID]),
	CONSTRAINT [FK_CustomGeoRegion_Pubs] FOREIGN KEY (PubID) REFERENCES [dbo].[Pubs]([PubID])
)
