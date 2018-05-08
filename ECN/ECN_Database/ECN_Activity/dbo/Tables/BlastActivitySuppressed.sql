CREATE TABLE [dbo].[BlastActivitySuppressed] (
    [SuppressID]       INT      IDENTITY (1, 1) NOT NULL,
    [BlastID]          INT      NOT NULL,
    [EmailID]          INT      NOT NULL,
    [SuppressedCodeID] INT      NOT NULL,
    [EAID]             INT      NULL,
    [SuppressedTime]   DATETIME NULL,
    [BlastsAlreadySent] VARCHAR(355) NULL, 
    CONSTRAINT [PK_BlastActivitySuppressed] PRIMARY KEY CLUSTERED ([SuppressID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_BlastActivitySuppressed_SuppressedCodes] FOREIGN KEY ([SuppressedCodeID]) REFERENCES [dbo].[SuppressedCodes] ([SuppressedCodeID])
);






GO
CREATE NONCLUSTERED INDEX [IX_BlastActivitySuppressed_EmailID]
    ON [dbo].[BlastActivitySuppressed]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BLASTACTIVITYSUPPRESSED_BLASTID_EMAILID_2]
    ON [dbo].[BlastActivitySuppressed]([BlastID] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivitySuppressed_SuppressID]
    ON [dbo].[BlastActivitySuppressed]([BlastID] ASC, [SuppressID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivitySuppressed_BlastId_SuppressedCodeId_EmailId]
    ON [dbo].[BlastActivitySuppressed]([BlastID] ASC, [SuppressedCodeID] ASC, [EmailID] ASC);

