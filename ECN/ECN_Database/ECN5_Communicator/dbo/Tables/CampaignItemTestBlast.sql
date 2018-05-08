CREATE TABLE [dbo].[CampaignItemTestBlast] (
    [CampaignItemTestBlastID] INT      IDENTITY (1, 1) NOT NULL,
    [CampaignItemID]          INT      NULL,
    [GroupID]                 INT      NULL,
    [HasEmailPreview]         BIT      NULL,
    [BlastID]                 INT      NULL,
    [CreatedUserID]           INT      NULL,
    [CreatedDate]             DATETIME CONSTRAINT [DF_CampaignItemTestBlast_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID]           INT      NULL,
    [UpdatedDate]             DATETIME NULL,
    [IsDeleted]               BIT      CONSTRAINT [DF_CampaignItemTestBlast_IsDeleted] DEFAULT ((0)) NULL,
    [FilterID]                INT      NULL,
    [CampaignItemTestBlastType] VARCHAR(50) NULL,
	[LayoutID]                INT      NULL, 
	[EmailSubject]                 VARCHAR (255)  NULL,
	[FromName]                 VARCHAR (255)  NULL,
    [FromEmail]                VARCHAR (100)  NULL,
    [ReplyTo]                  VARCHAR (100)  NULL,
    CONSTRAINT [PK_CampaignItemTestBlast] PRIMARY KEY CLUSTERED ([CampaignItemTestBlastID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CampaignItemTestBlast_CampaignItem] FOREIGN KEY ([CampaignItemID]) REFERENCES [dbo].[CampaignItem] ([CampaignItemID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CampaignItemTestBlast_blastID]
    ON [dbo].[CampaignItemTestBlast]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [FK_CampaignItemTestBlast_CampaignItem]
    ON [dbo].[CampaignItemTestBlast]([CampaignItemID] ASC) WITH (FILLFACTOR = 80);

