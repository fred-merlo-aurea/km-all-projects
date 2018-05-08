CREATE TABLE [dbo].[ChannelMasterSuppressionList] (
    [CMSID]         INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID] INT           NOT NULL,
    [EmailAddress]  VARCHAR (100) NOT NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_ChannelMasterSuppressionList_DateAdded] DEFAULT (getdate()) NOT NULL,
    [CreatedUserID] INT           NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_ChannelMasterSuppressionList_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ChannelMasterSuppressionList] PRIMARY KEY CLUSTERED ([CMSID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [BaseChannelEmailAndID]
    ON [dbo].[ChannelMasterSuppressionList]([BaseChannelID] ASC, [EmailAddress] ASC) WITH (FILLFACTOR = 80);

