CREATE TABLE [dbo].[BlastActivityOpensInternal]
(
	[OpenId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BlastID] INT NULL, 
    [EmailID] INT NULL, 
    [OpenTime] DATETIME NULL, 
    [BrowserInfo] VARCHAR(2048) NULL, 
    [EmailClientID] INT NULL, 
    [PlatformID] INT NULL
)
