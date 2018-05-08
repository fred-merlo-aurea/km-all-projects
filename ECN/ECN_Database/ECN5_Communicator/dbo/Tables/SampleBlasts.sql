CREATE TABLE [dbo].[SampleBlasts] (
    [SampleBlastID] INT IDENTITY (1, 1) NOT NULL,
    [SampleID]      INT NULL,
    [BlastID]       INT NULL,
    [Amount]        INT NULL,
    [IsAmount]      BIT NULL,
    CONSTRAINT [PK_SampleBlasts] PRIMARY KEY CLUSTERED ([SampleBlastID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_SampleBlasts_Blasts] FOREIGN KEY ([BlastID]) REFERENCES [dbo].[Blast] ([BlastID]),
    CONSTRAINT [FK_SampleBlasts_Samples] FOREIGN KEY ([SampleID]) REFERENCES [dbo].[Sample] ([SampleID])
);


GO
CREATE NONCLUSTERED INDEX [IX_SampleBlasts_BlastID]
    ON [dbo].[SampleBlasts]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[SampleBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SampleBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SampleBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SampleBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SampleBlasts] TO [reader]
    AS [dbo];

