CREATE TABLE [dbo].[CampaignItemBlast] (
    [CampaignItemBlastID] INT           IDENTITY (1, 1) NOT NULL,
    [CampaignItemID]      INT           NULL,
    [EmailSubject]        VARCHAR (255) NULL,
    [DynamicFromName]     VARCHAR (100) NULL,
    [DynamicFromEmail]    VARCHAR (100) NULL,
    [DynamicReplyTo]      VARCHAR (100) NULL,
    [LayoutID]            INT           NULL,
    [GroupID]             INT           NULL,
    [SocialMediaID]       INT           NULL,
    [FilterID]            INT           NULL,
    [SmartSegmentID]      INT           NULL,
    [BlastID]             INT           NULL,
    [CreatedUserID]       INT           NULL,
    [CreatedDate]         DATETIME      CONSTRAINT [DF_CampaignItemBlast_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID]       INT           NULL,
    [UpdatedDate]         DATETIME      NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_CampaignItemBlast_IsDeleted] DEFAULT ((0)) NULL,
    [AddOptOuts_to_MS]    BIT           NULL,
    [EmailFrom] VARCHAR(100) NULL, 
    [ReplyTo] VARCHAR(100) NULL, 
    [FromName] VARCHAR(100) NULL, 
    CONSTRAINT [PK_CampaignItemBlast] PRIMARY KEY CLUSTERED ([CampaignItemBlastID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CampaignItemBlast_CampaignItem] FOREIGN KEY ([CampaignItemID]) REFERENCES [dbo].[CampaignItem] ([CampaignItemID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CampaignItemBlast_blastID]
    ON [dbo].[CampaignItemBlast]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [FK_CampaignItemBlast_CampaignItem]
    ON [dbo].[CampaignItemBlast]([CampaignItemID] ASC) WITH (FILLFACTOR = 80);

