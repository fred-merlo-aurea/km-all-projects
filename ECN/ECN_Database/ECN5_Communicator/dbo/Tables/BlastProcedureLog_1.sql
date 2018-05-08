CREATE TABLE [dbo].[BlastProcedureLog] (
    [BlastProcedureLogId] INT              IDENTITY (1, 1) NOT NULL,
    [LoggingSet]          UNIQUEIDENTIFIER NULL,
    [CustomerId]          INT              NULL,
    [GroupId]             INT              NULL,
    [FilterID]            VARCHAR (MAX)    NULL,
    [ActionType]          VARCHAR (MAX)    NULL,
    [SupressionList]      VARCHAR (MAX)    NULL,
    [SourceProcedureName] VARCHAR (50)     NULL,
    [SourceBlastID]       INT              NULL,
    [ProcedureName]       VARCHAR (50)     NULL,
    [BlastId]             INT              NULL,
    [CampaignItemId]      INT              NULL,
    [StartTime]           DATETIME         NULL,
    [EndTime]             DATETIME         NULL,
    CONSTRAINT [PK__BlastPro__CE88C6B80FD8179B] PRIMARY KEY CLUSTERED ([BlastProcedureLogId] ASC)
);

