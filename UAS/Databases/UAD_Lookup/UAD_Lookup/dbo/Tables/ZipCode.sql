CREATE TABLE [dbo].[ZipCode]
(
	[ZipCodeId] [int] IDENTITY(1,1) NOT NULL,
	[ZipCodeTypeId] [int] NULL,
	[Zip] [varchar](9) NULL,
	[PrimaryCity] [varchar](50) NULL,
	[RegionId] [int] NULL,
	[CountryId] [int] NULL,
	[County] [varchar](50) NULL,
	[AreaCodes] [varchar](50) NULL,
	CONSTRAINT [PK_ZipCode] PRIMARY KEY CLUSTERED 
	(
		[ZipCodeId] ASC
	)
) ON [PRIMARY]
