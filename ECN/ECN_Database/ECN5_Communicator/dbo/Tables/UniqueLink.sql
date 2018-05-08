CREATE TABLE [dbo].[UniqueLink] (
    [UniqueLinkID] INT          IDENTITY (1, 1) NOT NULL,
    [BlastLinkID]  INT          NULL,
    [UniqueID]     VARCHAR (50) NULL,
    [BlastID]      INT          NULL,
    PRIMARY KEY CLUSTERED ([UniqueLinkID] ASC)
);



GO
CREATE NONCLUSTERED INDEX [IX_UniqueLink_BlastLinkID_UniqueID]
    ON [dbo].[UniqueLink]([BlastLinkID] ASC, [UniqueID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UniqueLink_BlastId_UniqueId]
    ON [dbo].[UniqueLink]([BlastID] ASC, [UniqueID] ASC);

