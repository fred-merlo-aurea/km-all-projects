CREATE TABLE [dbo].[ShoppingCarts] (
    [ShoppingCartID] INT              IDENTITY (1, 1) NOT NULL,
    [UserID]         UNIQUEIDENTIFIER NOT NULL,
    [SearchTypeID]   INT              NOT NULL,
    [SubscriptionID] INT              NOT NULL,
    [Price]          DECIMAL (10, 2)  NULL,
    [DateAdded]      DATETIME         CONSTRAINT [DF_ShoppingCart_DateAdded] DEFAULT (getdate()) NOT NULL,
    [IsFreeDownload] BIT              CONSTRAINT [DF_ShoppingCarts_IsFreeDownload] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ShoppingCart] PRIMARY KEY CLUSTERED ([ShoppingCartID] ASC),
    CONSTRAINT [FK_ShoppingCart_SearchType] FOREIGN KEY ([SearchTypeID]) REFERENCES [dbo].[SearchType] ([SearchTypeID]),
    CONSTRAINT [FK_ShoppingCart_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID]),
    CONSTRAINT [FK_ShoppingCarts_ApplicationUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[ApplicationUsers] ([UserID])
);

