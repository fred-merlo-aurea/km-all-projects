CREATE TABLE [dbo].[Campaigns] (
    [CampaignID]   INT           IDENTITY (1, 1) NOT NULL,
    [CampaignName] VARCHAR (100) NOT NULL,
    [AddedBy]      INT           NOT NULL,
    [DateAdded]    DATETIME      CONSTRAINT [DF_Campaigns_DateAdded] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]    INT           NOT NULL,
    [DateUpdated]  DATETIME      CONSTRAINT [DF_Campaigns_DateUpdated] DEFAULT (getdate()) NOT NULL,
    [BrandID]      INT           NULL,
    CONSTRAINT [PK_Campaigns] PRIMARY KEY CLUSTERED ([CampaignID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Campaigns_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);
GO
CREATE NONCLUSTERED INDEX [IDX_Campaigns_CampaignName]
    ON [dbo].[Campaigns]([CampaignName] ASC) WITH (FILLFACTOR = 90);
GO
