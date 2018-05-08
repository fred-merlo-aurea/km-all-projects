CREATE TABLE [dbo].[BrandDetails] (
    [BrandDetailsID] INT IDENTITY (1, 1) NOT NULL,
    [BrandID]        INT NOT NULL,
    [PubID]          INT NOT NULL,
    [GroupsBrandID]  INT NULL,
    CONSTRAINT [PK_BrandDetails] PRIMARY KEY CLUSTERED ([BrandDetailsID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BrandDetails_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);
GO

CREATE NONCLUSTERED INDEX [IX_BrandDetails_BrandID_PubID]
    ON [dbo].[BrandDetails]([BrandID] ASC, [PubID] ASC) WITH (FILLFACTOR = 90);
GO
