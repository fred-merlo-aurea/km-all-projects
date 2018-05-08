CREATE TABLE [dbo].[BlastActivityClicksInternal]
(
	[ClickId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BlastID] INT NULL, 
    [EmailID] INT NULL, 
    [URL] VARCHAR(2048) NULL, 
    [BlastLinkID] INT NULL, 
    [UniqueLinkID] INT NULL, 
    [ClickTime] DATETIME NULL
)
