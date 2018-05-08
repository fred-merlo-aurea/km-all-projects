CREATE TABLE [dbo].[CANON_PAIDPUB_eNewsletters] (
    [GroupID]     INT NOT NULL,
    [FrequencyID] INT NULL,
    [CustomerID]  INT NOT NULL,
    [CategoryID]  INT NULL,
    CONSTRAINT [FK_CANON_PAIDPUB_eNewsletters_CANON_PAIDPIB_eNewsletter_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[CANON_PAIDPUB_eNewsletter_Category] ([CategoryID])
);

