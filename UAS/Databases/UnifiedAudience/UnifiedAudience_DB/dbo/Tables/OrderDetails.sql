CREATE TABLE [dbo].[OrderDetails] (
    [OrderDetailsID] INT              IDENTITY (1, 1) NOT NULL,
    [OrderID]        INT              NOT NULL,
    [UserID]         UNIQUEIDENTIFIER NOT NULL,
    [SearchTypeID]   INT              NOT NULL,
    [SubscriptionID] INT              NOT NULL,
    [Price]          DECIMAL (10, 2)  CONSTRAINT [DF_OrderDetails_Price] DEFAULT ((0)) NOT NULL,
    [IsFreeDownload] BIT              CONSTRAINT [DF_OrderDetails_IsFreeDownload] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED ([OrderDetailsID] ASC),
    CONSTRAINT [FK_OrderDetails_ApplicationUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[ApplicationUsers] ([UserID]),
    CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Orders] ([OrderID]),
    CONSTRAINT [FK_OrderDetails_SearchType] FOREIGN KEY ([SearchTypeID]) REFERENCES [dbo].[SearchType] ([SearchTypeID]),
    CONSTRAINT [FK_OrderDetails_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
);

