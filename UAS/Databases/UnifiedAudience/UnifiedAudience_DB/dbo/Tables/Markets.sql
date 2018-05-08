CREATE TABLE [dbo].[Markets] (
    [MarketID]        INT                          IDENTITY (1, 1) NOT NULL,
    [MarketName]      VARCHAR (100)                NULL,
    [MarketXML]       XML(CONTENT [dbo].[Markets]) NULL,
    [BrandID]         INT                          NULL,
    [DateCreated]     DATETIME                     CONSTRAINT [DF_Markets_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME                     NULL,
    [CreatedByUserID] INT                          NULL,
    [UpdatedByUserID] INT                          NULL,
    CONSTRAINT [PK_Markets] PRIMARY KEY CLUSTERED ([MarketID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Markets_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Markets_MarketName]
    ON [dbo].[Markets]([MarketName] ASC) WITH (FILLFACTOR = 70);
GO
