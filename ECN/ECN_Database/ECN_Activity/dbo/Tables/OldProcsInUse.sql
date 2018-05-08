CREATE TABLE [dbo].[OldProcsInUse] (
    [OldProcsID] INT           IDENTITY (1, 1) NOT NULL,
    [ProcName]   VARCHAR (500) NOT NULL,
    [DateUsed]   DATETIME      NOT NULL,
    CONSTRAINT [PK_spOldProcsInUse] PRIMARY KEY CLUSTERED ([OldProcsID] ASC) WITH (FILLFACTOR = 80)
);

