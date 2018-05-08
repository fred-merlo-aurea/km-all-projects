CREATE TABLE [dbo].[BlastEmails] (
    [BlastId] INT NOT NULL,
    [EmailID] INT NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [IDXBlastEmails]
    ON [dbo].[BlastEmails]([BlastId] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80);

