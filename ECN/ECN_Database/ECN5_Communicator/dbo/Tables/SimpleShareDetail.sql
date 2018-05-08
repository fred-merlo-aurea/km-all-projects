CREATE TABLE [dbo].[SimpleShareDetail]
(
	[SimpleShareDetailID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SocialMediaID] INT NULL, 
    [SocialMediaAuthID] INT NULL, 
    [Title] VARCHAR(200) NULL, 
    [SubTitle] VARCHAR(200) NULL, 
    [ImagePath] VARCHAR(200) NULL, 
    [Content] VARCHAR(200) NULL, 
    [CampaignItemID] INT NULL, 
    [UseThumbnail] BIT NULL, 
    [CreatedUserID] INT NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [IsDeleted] BIT NULL, 
    [PageAccessToken] VARCHAR(MAX) NULL, 
    [PageID] VARCHAR(MAX) NULL
)
