CREATE TABLE [dbo].[SocialMediaAuth]
(
	[SocialMediaAuthId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SocialMediaID] INT NOT NULL, 
    [CustomerID] INT NULL, 
    [Access_Token] VARCHAR(500) NULL, 
    [UserID] VARCHAR(500) NULL, 
    [CreatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [IsDeleted] BIT NULL, 
    [Access_Secret] VARCHAR(500) NULL, 
    [ProfileName] VARCHAR(500) NULL
)
