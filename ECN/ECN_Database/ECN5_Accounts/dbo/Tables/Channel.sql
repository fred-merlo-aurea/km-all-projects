CREATE TABLE [dbo].[Channel] (
    [ChannelID]       INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]   INT           NULL,
    [ChannelName]     VARCHAR (50)  NULL,
    [AssetsPath]      VARCHAR (255) NULL,
    [VirtualPath]     VARCHAR (100) NULL,
    [PickupPath]      VARCHAR (255) NULL,
    [HeaderSource]    TEXT          NULL,
    [FooterSource]    TEXT          NULL,
    [ChannelTypeCode] VARCHAR (50)  NULL,
    [Active]          CHAR (1)      NULL,
    [MailingIP]       VARCHAR (50)  NULL,
    [CreatedUserID]   INT           NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_Channel_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]   INT           NULL,
    [UpdatedDate]     DATETIME      NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_Channels_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [Channels_PK] PRIMARY KEY CLUSTERED ([ChannelID] ASC) WITH (FILLFACTOR = 80)
);

