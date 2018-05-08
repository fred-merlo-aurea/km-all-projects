CREATE TABLE [dbo].[BrandScore] (
    [BrandScoreId]   INT           IDENTITY (1, 1) NOT NULL,
    [SubscriptionId] INT           NOT NULL,
    [BrandId]        INT           NOT NULL,
    [Score]          INT           NOT NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_BrandScore_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreateUser]     VARCHAR (250) CONSTRAINT [DF_BrandScore_CreateUser] DEFAULT (suser_sname()) NOT NULL,
    [UpdateDate]     DATETIME      NULL,
    [UpdateUser]     VARCHAR (250) NULL,
    CONSTRAINT [PK_BrandScore] PRIMARY KEY CLUSTERED ([BrandScoreId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BrandScore_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandID])
);
GO

CREATE NONCLUSTERED INDEX [IDX_BrandScore_SubscriptionID]
    ON [dbo].[BrandScore]([SubscriptionId] ASC) WITH (FILLFACTOR = 90);
GO
CREATE INDEX [IX_BrandScore_BrandID] ON [dbo].[BrandScore] ([BrandId])
go