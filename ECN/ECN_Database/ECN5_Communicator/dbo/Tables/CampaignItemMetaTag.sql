CREATE TABLE [dbo].[CampaignItemMetaTag]
(
	[CampaignItemMetaTagID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CampaignItemID] INT NULL, 
    [SocialMediaID] INT NULL, 
    [Property] VARCHAR(100) NULL, 
    [Content] VARCHAR(MAX) NULL, 
    [CreatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [IsDeleted] BIT NULL
)
