CREATE TABLE [dbo].[CustomerProduct] (
    [CustomerProductID] INT      IDENTITY (100, 1) NOT NULL,
    [CustomerID]        INT      NULL,
    [ProductDetailID]   INT      NULL,
    [Active]            CHAR (1) CONSTRAINT [DF_CustomerProducts_Active] DEFAULT ('n') NOT NULL,
    [UpdatedDate]       DATETIME NULL,
    [CreatedDate]       DATETIME CONSTRAINT [DF_CustomerProduct_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedUserID]     INT      NULL,
    [IsDeleted]         BIT      CONSTRAINT [DF_CustomerProduct_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID]     INT      NULL,
    CONSTRAINT [PK_CustomerProducts] PRIMARY KEY CLUSTERED ([CustomerProductID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerProducts_CustomeriD_Active]
    ON [dbo].[CustomerProduct]([CustomerID] ASC, [Active] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerProducts_CustomerID_ProductDetailID_Active]
    ON [dbo].[CustomerProduct]([CustomerID] ASC)
    INCLUDE([ProductDetailID], [Active]) WITH (FILLFACTOR = 80);

