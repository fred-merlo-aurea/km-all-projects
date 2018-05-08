CREATE TABLE [dbo].[channel_digitalriver_backup] (
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
    [MailingIP]       VARCHAR (50)  NULL
);

