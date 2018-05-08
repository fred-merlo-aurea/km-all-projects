CREATE TABLE [dbo].[CampaignFilter] (
    [CampaignFilterID] INT           IDENTITY (1, 1) NOT NULL,
    [CampaignID]       INT           NOT NULL,
    [FilterName]       VARCHAR (500) NULL,
    [AddedBy]          INT           NOT NULL,
    [DateAdded]        DATETIME      CONSTRAINT [DF_CampaignFilter_DateAdded] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]        INT           NOT NULL,
    [DateUpdated]      DATETIME      CONSTRAINT [DF_CampaignFilter_DateUpdated] DEFAULT (getdate()) NOT NULL,
    [PromoCode]        VARCHAR (50)  NULL,
    CONSTRAINT [PK_CampaignFilter] PRIMARY KEY CLUSTERED ([CampaignFilterID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CampaignFilter_Campaigns] FOREIGN KEY ([CampaignID]) REFERENCES [dbo].[Campaigns] ([CampaignID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_CampaignFilter_CampaignID]
    ON [dbo].[CampaignFilter]([CampaignID] ASC) WITH (FILLFACTOR = 90);
GO