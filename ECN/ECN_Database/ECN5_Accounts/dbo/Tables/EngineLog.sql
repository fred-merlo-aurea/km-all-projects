CREATE TABLE [dbo].[EngineLog] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [EngineName]   VARCHAR (20)   NOT NULL,
    [ActionDate]   DATETIME       NOT NULL,
    [Action]       VARCHAR (500)  NOT NULL,
    [Status]       VARCHAR (20)   NOT NULL,
    [ErrorMessage] VARCHAR (2000) NULL,
    [IsAcked]      BIT            CONSTRAINT [DF_EngineLog_IsAcked] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_EngineLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

