CREATE TABLE [dbo].[ChannelSeedList] (
    [CSLID]         INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID] INT           NOT NULL,
    [Emailaddress]  VARCHAR (100) NOT NULL,
    [DateAdded]     DATETIME      NOT NULL,
    CONSTRAINT [PK_ChannelSeedList] PRIMARY KEY CLUSTERED ([CSLID] ASC) WITH (FILLFACTOR = 80)
);

