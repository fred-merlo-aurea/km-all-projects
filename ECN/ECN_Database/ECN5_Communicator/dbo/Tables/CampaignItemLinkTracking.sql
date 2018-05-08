CREATE TABLE [dbo].[CampaignItemLinkTracking] (
    [CILTID]         INT           IDENTITY (1, 1) NOT NULL,
    [CampaignItemID] INT           NULL,
    [LTPID]          INT           NULL,
    [LTPOID]         INT           NULL,
    [CustomValue]    VARCHAR (255) NULL,
    CONSTRAINT [PK_CampaignItemLinkTracking] PRIMARY KEY CLUSTERED ([CILTID] ASC) WITH (FILLFACTOR = 80)
);

