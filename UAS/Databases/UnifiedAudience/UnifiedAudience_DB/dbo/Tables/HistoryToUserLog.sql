CREATE TABLE [dbo].[HistoryToUserLog] (
    [HistoryID] INT NOT NULL,
    [UserLogID] INT NOT NULL,
    CONSTRAINT [PK_HistoryMap] PRIMARY KEY CLUSTERED ([HistoryID] ASC, [UserLogID] ASC) WITH (FILLFACTOR = 80)
);

