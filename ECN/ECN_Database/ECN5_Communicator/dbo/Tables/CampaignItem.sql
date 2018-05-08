CREATE TABLE [dbo].[CampaignItem] (
    [CampaignItemID]           INT            IDENTITY (1, 1) NOT NULL,
    [CampaignID]               INT            NULL,
    [CampaignItemName]         VARCHAR (255)  NULL,
    [CampaignItemType]         VARCHAR (20)   NULL,
    [FromName]                 VARCHAR (255)  NULL,
    [FromEmail]                VARCHAR (100)  NULL,
    [ReplyTo]                  VARCHAR (100)  NULL,
    [SendTime]                 DATETIME       NULL,
    [SampleID]                 INT            NULL,
    [PageWatchID]              INT            NULL,
    [BlastScheduleID]          INT            NULL,
    [OverrideAmount]           INT            NULL,
    [OverrideIsAmount]         BIT            NULL,
    [BlastField1]              VARCHAR (255)  NULL,
    [BlastField2]              VARCHAR (255)  NULL,
    [BlastField3]              VARCHAR (255)  NULL,
    [BlastField4]              VARCHAR (255)  NULL,
    [BlastField5]              VARCHAR (255)  NULL,
    [CompletedStep]            INT            NULL,
    [CreatedUserID]            INT            NULL,
    [CreatedDate]              DATETIME       CONSTRAINT [DF_CampaignItem_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID]            INT            NULL,
    [UpdatedDate]              DATETIME       NULL,
    [IsDeleted]                BIT            CONSTRAINT [DF_CampaignItem_IsDeleted] DEFAULT ((0)) NULL,
    [CampaignItemFormatType]   VARCHAR (10)   NULL,
    [IsHidden]                 BIT            NULL,
    [CampaignItemNameOriginal] VARCHAR (255)  NULL,
    [CampaignItemIDOriginal]   INT            NULL,
    [NodeID]                   VARCHAR (1000) NULL,
    [CampaignItemTemplateID]   INT            NULL,
    [EnableCacheBuster]        BIT            NULL,
    [SFCampaignID]             VARCHAR (500)  NULL,
	[IgnoreSuppression]		   BIT			  NULL,
    CONSTRAINT [PK_CampaignItem_NEW] PRIMARY KEY CLUSTERED ([CampaignItemID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CampaignItem_Campaign] FOREIGN KEY ([CampaignID]) REFERENCES [dbo].[Campaign] ([CampaignID])
);




GO
CREATE NONCLUSTERED INDEX [FK_CampaignItem_Campaign]
    ON [dbo].[CampaignItem]([CampaignID] ASC) WITH (FILLFACTOR = 80);

