CREATE TABLE [dbo].[ProductDetail] (
    [ProductDetailID]   INT           IDENTITY (100, 1) NOT NULL,
    [ProductID]         INT           NULL,
    [ProductDetailName] VARCHAR (50)  NULL,
    [ProductDetailDesc] VARCHAR (255) NULL,
    CONSTRAINT [PK_ProductDetail] PRIMARY KEY CLUSTERED ([ProductDetailID] ASC) WITH (FILLFACTOR = 80)
);

