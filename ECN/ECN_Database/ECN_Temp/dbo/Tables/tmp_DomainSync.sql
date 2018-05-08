CREATE TABLE [dbo].[tmp_DomainSync] (
    [subscriptionID] INT           NULL,
    [Email]          VARCHAR (100) NULL
);


GO
CREATE CLUSTERED INDEX [IDX_tmpMAF_DomainSync_Email]
    ON [dbo].[tmp_DomainSync]([Email] ASC);

