CREATE TABLE [dbo].[PubMarkets] (
    [PubID]    INT NOT NULL,
    [MarketID] INT NOT NULL,
    CONSTRAINT [PK_PubMarkets] PRIMARY KEY CLUSTERED ([PubID] ASC, [MarketID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PubMarkets_Markets] FOREIGN KEY ([MarketID]) REFERENCES [dbo].[Markets] ([MarketID]),
    CONSTRAINT [FK_PubMarkets_PubMarkets] FOREIGN KEY ([PubID], [MarketID]) REFERENCES [dbo].[PubMarkets] ([PubID], [MarketID]),
    CONSTRAINT [FK_PubMarkets_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID])
);
GO
CREATE NONCLUSTERED INDEX [IDX_PubMarkets]
    ON [dbo].[PubMarkets]([PubID] ASC) WITH (FILLFACTOR = 70);
GO
