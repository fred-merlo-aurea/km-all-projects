CREATE TABLE [dbo].[HistoryToHistoryMarketingMap] (
    [HistoryID]             INT NOT NULL,
    [HistoryMarketingMapID] INT NOT NULL,
    CONSTRAINT [PK_HistoryToHistoryMaketingMap] PRIMARY KEY CLUSTERED ([HistoryID] ASC, [HistoryMarketingMapID] ASC) WITH (FILLFACTOR = 80)
);

