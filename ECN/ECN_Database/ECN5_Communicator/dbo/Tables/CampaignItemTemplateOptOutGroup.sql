CREATE TABLE [dbo].[CampaignItemTemplateOptOutGroup] (
	[CampaignItemTemplateOptOutGroupID]		 INT      IDENTITY (1, 1) NOT NULL,
    [CampaignItemTemplateID]                 INT      NULL,
    [GroupID]                                INT      NULL,
    [CreatedDate]                            DATETIME NULL,
    [CreatedUserID]                          INT      NULL,
    [UpdatedUserID]                          INT      NULL,
    [UpdatedDate]                            DATETIME NULL,
    [IsDeleted]                              BIT      NULL,
    CONSTRAINT [PK_CampaignItemTemplateOptOutGroup] PRIMARY KEY CLUSTERED ([CampaignItemTemplateOptOutGroupID] ASC)
);
