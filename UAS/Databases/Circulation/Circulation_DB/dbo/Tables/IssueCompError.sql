CREATE TABLE [dbo].[IssueCompError]
(
	[IssueCompErrorID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[CompName] VARCHAR(200) NOT NULL, 
	[SFRecordIdentifier] UNIQUEIDENTIFIER NOT NULL, 
	[ProcessCode] VARCHAR(50) NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL, 
    [CreatedByUserID] INT NOT NULL, 
    [UpdatedByUserID] INT NULL
)
