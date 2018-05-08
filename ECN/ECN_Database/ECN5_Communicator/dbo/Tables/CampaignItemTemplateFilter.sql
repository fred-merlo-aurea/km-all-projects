CREATE TABLE [dbo].[CampaignItemTemplateFilter](
	[CampaignItemTemplateFilterID]			 INT      IDENTITY (1, 1) NOT NULL,
	[CampaignItemTemplateID]				 INT	  NULL,
    [GroupID]								 INT      NULL,
    [FilterID]                               INT      NULL,
    [CreatedDate]                            DATETIME NULL,
    [CreatedUserID]                          INT      NULL,
    [UpdatedUserID]                          INT      NULL,
    [UpdatedDate]                            DATETIME NULL,
    [IsDeleted]                              BIT      NULL,
    CONSTRAINT [PK_CampaignItemTemplateFilter] PRIMARY KEY CLUSTERED ([CampaignItemTemplateFilterID] ASC)
);
