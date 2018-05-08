CREATE TABLE [dbo].[ChampionAudit] (
    [ChampionAuditID] INT          IDENTITY (1, 1) NOT NULL,
    [AuditTime]       DATETIME     NULL,
    [SampleID]        INT          NULL,
    [BlastIDA]        INT          NULL,
    [BlastIDB]        INT          NULL,
    [BlastIDChampion] INT          NULL,
    [ClicksA]         INT          NULL,
    [ClicksB]         INT          NULL,
    [OpensA]          INT          NULL,
    [OpensB]          INT          NULL,
    [BouncesA]        INT          NULL,
    [BouncesB]        INT          NULL,
    [BlastIDWinning]  INT          NULL,
    [SendToNonWinner] BIT          NULL,
    [Reason]          VARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([ChampionAuditID] ASC)
);

