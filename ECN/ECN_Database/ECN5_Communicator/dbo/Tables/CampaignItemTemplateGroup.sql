CREATE TABLE [dbo].[CampaignItemTemplateGroup]
(
	[CampaignItemTemplateGroupId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CampaignItemTemplateID] INT NULL, 
    [GroupID] INT NULL, 
    [IsDeleted] BIT NULL, 
    [CreatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL
)
