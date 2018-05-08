CREATE TABLE [dbo].[APILogging] (
    [APILogID]  INT              IDENTITY (1, 1) NOT NULL,
    [AccessKey] UNIQUEIDENTIFIER NULL,
    [APIMethod] VARCHAR (255)    NULL,
    [Input]     TEXT             NULL,
    [StartTime] DATETIME         NULL,
    [LogID]     INT              NULL,
    [EndTime]   DATETIME         NULL,
    CONSTRAINT [PK_APILogging] PRIMARY KEY CLUSTERED ([APILogID] ASC) WITH (FILLFACTOR = 80)
);

