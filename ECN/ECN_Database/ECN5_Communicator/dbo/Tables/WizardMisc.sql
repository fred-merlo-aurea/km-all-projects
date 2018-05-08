CREATE TABLE [dbo].[WizardMisc] (
    [WizardMiscitemsID] INT          IDENTITY (1, 1) NOT NULL,
    [WizardID]          VARCHAR (50) NULL,
    [CustRefrence]      VARCHAR (50) CONSTRAINT [DF_WizardMisc_CustRefrence] DEFAULT ('') NULL,
    [BillingCode]       VARCHAR (50) CONSTRAINT [DF_WizardMisc_BillingCode] DEFAULT ('') NULL,
    [FolderID]          INT          CONSTRAINT [DF_WizardMisc_FolderID] DEFAULT (0) NULL,
    [BlastCodeID]       INT          CONSTRAINT [DF_WizardMisc_BlastCodeID] DEFAULT (0) NULL,
    [BlastGroupID]      INT          NULL,
    [WizardTemplateID]  INT          NULL,
    [CampaignItemID]    INT          NULL,
    CONSTRAINT [PK_WizardMisc] PRIMARY KEY CLUSTERED ([WizardMiscitemsID] ASC) WITH (FILLFACTOR = 80)
);

