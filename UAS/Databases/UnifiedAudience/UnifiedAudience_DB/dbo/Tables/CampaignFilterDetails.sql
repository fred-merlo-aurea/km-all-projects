CREATE TABLE [dbo].[CampaignFilterDetails] (
    [CampaignFilterDetailsID] INT IDENTITY (1, 1) NOT NULL,
    [CampaignFilterID]        INT NOT NULL,
    [SubscriptionID]          INT NOT NULL,
    CONSTRAINT [FK_CampaignFilterDetails_CampaignFilter] FOREIGN KEY ([CampaignFilterID]) REFERENCES [dbo].[CampaignFilter] ([CampaignFilterID]),
    CONSTRAINT [FK_CampaignFilterDetails_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID]), 
    CONSTRAINT [PK_CampaignFilterDetails] PRIMARY KEY ([CampaignFilterDetailsID])
);
GO
CREATE NONCLUSTERED INDEX [IDX_CampaignFilterDetails_CampaignFilterID_SubscriptionID]
    ON [dbo].[CampaignFilterDetails]([CampaignFilterID] ASC, [SubscriptionID] ASC);
GO

CREATE NONCLUSTERED INDEX [IDX_CampaignFilterDetails_SubscriptionID]
    ON [dbo].[CampaignFilterDetails]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO