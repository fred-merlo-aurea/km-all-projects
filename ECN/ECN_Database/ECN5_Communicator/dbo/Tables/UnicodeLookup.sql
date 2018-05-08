CREATE TABLE [dbo].[UnicodeLookup]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Glyph] NVARCHAR(50) NULL, 
    [UnicodeNum] VARCHAR(255) NULL, 
    [HTML] VARCHAR(255) NOT NULL, 
    [IsEnabled] BIT NULL, 
    [Base64String] VARCHAR(MAX) NULL, 
    [UnicodeBytes] VARCHAR(MAX) NULL
)
