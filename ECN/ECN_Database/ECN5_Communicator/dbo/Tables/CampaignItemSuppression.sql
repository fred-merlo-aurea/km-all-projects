CREATE TABLE [dbo].[CampaignItemSuppression] (
    [CampaignItemSuppressionID] INT      IDENTITY (1, 1) NOT NULL,
    [CampaignItemID]            INT      NULL,
    [GroupID]                   INT      NULL,
    [CreatedUserID]             INT      NULL,
    [CreatedDate]               DATETIME CONSTRAINT [DF_CampaignItemSuppression_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID]             INT      NULL,
    [UpdatedDate]               DATETIME NULL,
    [IsDeleted]                 BIT      CONSTRAINT [DF_CampaignItemSuppression_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CampaignItemSuppression] PRIMARY KEY CLUSTERED ([CampaignItemSuppressionID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CampaignItemSuppression_CampaignItem] FOREIGN KEY ([CampaignItemID]) REFERENCES [dbo].[CampaignItem] ([CampaignItemID])
);

