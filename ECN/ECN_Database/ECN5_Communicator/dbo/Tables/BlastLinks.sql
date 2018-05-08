CREATE TABLE [dbo].[BlastLinks] (
    [BlastLinkID] INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]     INT            NOT NULL,
    [LinkURL]     VARCHAR (2048) NOT NULL,
    CONSTRAINT [PK_BlastLinks] PRIMARY KEY CLUSTERED ([BlastLinkID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_BlastLinks_BlastID_1]
    ON [dbo].[BlastLinks]([BlastID] ASC)
    INCLUDE([BlastLinkID], [LinkURL]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_BlastLinks_BlastID_BlastLinkID]
    ON [dbo].[BlastLinks]([BlastID] ASC)
    INCLUDE([BlastLinkID]) WITH (FILLFACTOR = 80);

