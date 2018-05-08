CREATE TABLE [dbo].[CampaignItemBlastRefBlast] (
    [CampaignItemBlastRefBlastID] INT      IDENTITY (1, 1) NOT NULL,
    [CampaignItemBlastID]         INT      NULL,
    [RefBlastID]                  INT      NULL,
    [CreatedUserID]               INT      NULL,
    [CreatedDate]                 DATETIME CONSTRAINT [DF_CampaignItemBlastRefBlast_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID]               INT      NULL,
    [UpdatedDate]                 DATETIME NULL,
    [IsDeleted]                   BIT      CONSTRAINT [DF_CampaignItemBlastRefBlast_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CampaignItemBlastRefBlast] PRIMARY KEY CLUSTERED ([CampaignItemBlastRefBlastID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CampaignItemBlastRefBlast_CampaignItemBlast] FOREIGN KEY ([CampaignItemBlastID]) REFERENCES [dbo].[CampaignItemBlast] ([CampaignItemBlastID])
);

