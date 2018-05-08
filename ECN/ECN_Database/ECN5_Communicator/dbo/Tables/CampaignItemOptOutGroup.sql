CREATE TABLE [dbo].[CampaignItemOptOutGroup] (
    [CampaignItemOptOutID] INT      IDENTITY (1, 1) NOT NULL,
    [CampaignItemID]       INT      NOT NULL,
    [GroupID]              INT      NOT NULL,
    [CreatedDate]          DATETIME CONSTRAINT [DF_BlastOptOutGroup_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]        INT      NULL,
    [IsDeleted]            BIT      CONSTRAINT [DF_BlastOptOutGroup_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]          DATETIME NULL,
    [UpdatedUserID]        INT      NULL,
    CONSTRAINT [PK_OptOut] PRIMARY KEY CLUSTERED ([CampaignItemOptOutID] ASC) WITH (FILLFACTOR = 80)
);

