CREATE TABLE [dbo].[EmailDirect]
(
	[EmailDirectID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerID] INT NULL, 
    [Source] VARCHAR(500) NULL, 
    [Process] VARCHAR(500) NULL, 
    [Status] VARCHAR(20) NULL, 
    [SendTime] DATETIME NULL, 
    [StartTime] DATETIME NULL, 
    [FinishTime] DATETIME NULL, 
    [EmailAddress] VARCHAR(500) NULL, 
    [FromEmailAddress] VARCHAR(500) NULL, 
    [FromName] VARCHAR(500) NULL, 
    [ReplyEmailAddress] VARCHAR(500) NULL, 
    [Content] VARCHAR(MAX) NULL, 
    [CreatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [OpenTime] DATETIME NULL, 
    [EmailSubject] VARCHAR(500) NULL
)
