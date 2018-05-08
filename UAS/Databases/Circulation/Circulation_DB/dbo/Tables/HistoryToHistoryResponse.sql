CREATE TABLE [dbo].[HistoryToHistoryResponse] (
    [HistoryID]         INT NOT NULL,
    [HistoryResponseID] INT NOT NULL,
    CONSTRAINT [PK_HistoryToHistoryResonse] PRIMARY KEY CLUSTERED ([HistoryID] ASC, [HistoryResponseID] ASC) WITH (FILLFACTOR = 80)
);

