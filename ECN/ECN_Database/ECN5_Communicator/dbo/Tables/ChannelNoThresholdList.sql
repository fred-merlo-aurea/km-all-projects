CREATE TABLE [dbo].[ChannelNoThresholdList] (
    [CNTID]         INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID] INT           NOT NULL,
    [EmailAddress]  VARCHAR (100) NOT NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_ChannelNoThresholdList_DateAdded] DEFAULT (getdate()) NOT NULL,
    [CreatedUserID] INT           NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_ChannelNoThresholdList_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ChannelNoThresholdList] PRIMARY KEY CLUSTERED ([CNTID] ASC) WITH (FILLFACTOR = 80)
);

