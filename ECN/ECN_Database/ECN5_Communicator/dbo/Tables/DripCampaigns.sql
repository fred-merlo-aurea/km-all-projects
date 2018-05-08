CREATE TABLE [dbo].[DripCampaigns] (
    [DripCampaignID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]         INT           NOT NULL,
    [CustomerID]     INT           NOT NULL,
    [JSONcode]       TEXT          NOT NULL,
    [CampaignName]   VARCHAR (100) NOT NULL
);

