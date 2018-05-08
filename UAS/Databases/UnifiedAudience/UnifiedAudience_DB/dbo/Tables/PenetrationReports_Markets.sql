CREATE TABLE [dbo].[PenetrationReports_Markets] (
    [PrmID]    INT IDENTITY (1, 1) NOT NULL,
    [ReportID] INT NOT NULL,
    [MarketID] INT NOT NULL,
    CONSTRAINT [PK_PenetrationReports_Markets] PRIMARY KEY CLUSTERED ([PrmID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PenetrationReports_Markets_Markets] FOREIGN KEY ([MarketID]) REFERENCES [dbo].[Markets] ([MarketID]),
    CONSTRAINT [FK_PenetrationReports_Markets_PenetrationReports] FOREIGN KEY ([ReportID]) REFERENCES [dbo].[PenetrationReports] ([ReportID])
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_PenetrationReports_Markets_ReportID_MarketID]
    ON [dbo].[PenetrationReports_Markets]([ReportID] ASC, [MarketID] ASC) WITH (FILLFACTOR = 90);
GO
