CREATE TABLE [dbo].[CampaignItemSocialMedia] (
    [CampaignItemSocialMediaID] INT           IDENTITY (1, 1) NOT NULL,
    [CampaignItemID]            INT           NULL,
    [SocialMediaID]             INT           NULL,
    [SimpleShareDetailID]       INT           NULL,
    [SocialMediaAuthID]         INT           NULL,
    [PageID]                    VARCHAR (MAX) NULL,
    [PageAccessToken]           VARCHAR (MAX) NULL,
    [Status]                    VARCHAR (10)  NULL,
    [StatusDate]                DATETIME      NULL,
    [PostID]                    VARCHAR (MAX) NULL,
    [LastErrorCode]             INT           NULL,
    PRIMARY KEY CLUSTERED ([CampaignItemSocialMediaID] ASC)
);


