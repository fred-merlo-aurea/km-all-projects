CREATE TABLE [dbo].[Tmp_EmailStatus] (
    [PubSubscriptionId] INT           NULL,
    [SubscriptionId]    INT           NULL,
    [EmailStatusID]     INT           NULL,
    [PubID]             INT           NULL,
    [EmailAddress]      VARCHAR (255) NULL,
    [GroupID]           INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IDX_Tmp_EmailStatus]
    ON [dbo].[Tmp_EmailStatus]([GroupID] ASC, [EmailAddress] ASC);

