CREATE TABLE [dbo].[ProcedureLogging] (
    [LogID]      INT          IDENTITY (1, 1) NOT NULL,
    [BlastID]    INT          NULL,
    [GroupID]    INT          NULL,
    [xmlProfile] TEXT         NULL,
    [xmlUDF]     TEXT         NULL,
    [StartTime]  DATETIME     NULL,
    [EndTime]    DATETIME     NULL,
    [ProcName]   VARCHAR (50) NULL,
    CONSTRAINT [PK_ProcedureLogging] PRIMARY KEY CLUSTERED ([LogID] ASC) WITH (FILLFACTOR = 80)
);

