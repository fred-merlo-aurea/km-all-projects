CREATE TABLE [dbo].[Tmp_PubSubscriptionsSync] (
    [GroupId]           INT           NULL,
    [PubId]             INT           NULL,
    [PubsubScriptionID] INT           NULL,
    [Email]             VARCHAR (100) NULL
);




GO
CREATE NONCLUSTERED INDEX [IDX_tmpPubSubscriptions_email]
    ON [dbo].[Tmp_PubSubscriptionsSync]([GroupId] ASC, [Email] ASC);



