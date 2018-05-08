CREATE TABLE [dbo].[ChampionBlasts] (
    [ChampionBlastID] INT IDENTITY (1, 1) NOT NULL,
    [BlastID]         INT NULL,
    [SampleID]        INT NULL,
    CONSTRAINT [PK_ChampionBlasts] PRIMARY KEY CLUSTERED ([ChampionBlastID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ChampionBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ChampionBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ChampionBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ChampionBlasts] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ChampionBlasts] TO [reader]
    AS [dbo];

