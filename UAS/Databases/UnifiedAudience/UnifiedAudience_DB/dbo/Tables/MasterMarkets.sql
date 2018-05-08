CREATE TABLE [dbo].[MasterMarkets] (
    [MarketID] INT NOT NULL,
    [MasterID] INT NOT NULL,
    CONSTRAINT [PK_MasterMarkets] PRIMARY KEY CLUSTERED ([MarketID] ASC, [MasterID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_MasterMarkets_Markets] FOREIGN KEY ([MarketID]) REFERENCES [dbo].[Markets] ([MarketID]),
    CONSTRAINT [FK_MasterMarkets_Mastercodesheet] FOREIGN KEY ([MasterID]) REFERENCES [dbo].[Mastercodesheet] ([MasterID])
);

