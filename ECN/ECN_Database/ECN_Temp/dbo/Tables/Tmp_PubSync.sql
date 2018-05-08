CREATE TABLE [dbo].[Tmp_PubSync] (
    [GroupId]         INT NULL,
    [PubId]           INT NULL,
    [SubscriberCount] INT NULL
);


GO
CREATE NONCLUSTERED INDEX [idx_Tmp_PubSync_GroupId]
    ON [dbo].[Tmp_PubSync]([GroupId] ASC);

