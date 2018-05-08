CREATE TABLE [dbo].[CampaignItemBlastFilter]
(
	[CampaignItemBlastFilterID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CampaignItemBlastID] INT NULL, 
    [CampaignItemSuppressionID] INT NULL, 
    [CampaignItemTestBlastID] INT NULL, 
    [SuppressionGroupID] INT NULL, 
    [SmartSegmentID] INT NULL, 
    [RefBlastIDs] VARCHAR(500) NULL, 
    [FilterID] INT NULL, 
    [IsDeleted] BIT NULL
)
