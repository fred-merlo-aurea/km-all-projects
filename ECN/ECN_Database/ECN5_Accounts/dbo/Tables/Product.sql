CREATE TABLE [dbo].[Product] (
    [ProductID]        INT          IDENTITY (100, 1) NOT NULL,
    [ProductName]      VARCHAR (50) NULL,
    [HasWebsiteTarget] BIT          CONSTRAINT [DF_Product_HasWebsiteTarget] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductID] ASC) WITH (FILLFACTOR = 80)
);

