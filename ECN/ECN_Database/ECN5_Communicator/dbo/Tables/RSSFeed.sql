CREATE TABLE [dbo].[RSSFeed]
(
	[FeedId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerID] INT NULL, 
    [Name] VARCHAR(100) NULL, 
    [URL] VARCHAR(500) NULL, 
    [StoriesToShow] INT NULL, 
    [CreatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [IsDeleted] BIT NULL
)
