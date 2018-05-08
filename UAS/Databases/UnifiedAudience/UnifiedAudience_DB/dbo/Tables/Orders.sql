﻿CREATE TABLE [dbo].[Orders] (
    [OrderID]              INT              IDENTITY (1, 1) NOT NULL,
    [UserID]               UNIQUEIDENTIFIER NOT NULL,
    [OrderDate]            DATETIME         NOT NULL,
    [OrderSubTotal]        DECIMAL (10, 2)  NULL,
    [PromotionCode]        VARCHAR (10)     NULL,
    [OrderTotal]           DECIMAL (10, 2)  NULL,
    [CardHolderName]       VARCHAR (50)     NULL,
    [CardHolderAddress1]   VARCHAR (100)    NULL,
    [CardHolderAddress2]   VARCHAR (100)    NULL,
    [CardHolderCity]       VARCHAR (50)     NULL,
    [CardHolderState]      VARCHAR (50)     NULL,
    [CardHolderZip]        VARCHAR (10)     NULL,
    [CardHolderCountryID]  INT              NULL,
    [CardExpirationMonth]  INT              NULL,
    [CardExpirationYear]   INT              NULL,
    [CardType]             VARCHAR (25)     NULL,
    [CardNo]               VARCHAR (4)      NULL,
    [CardCVV]              VARCHAR (10)     NULL,
    [IsProcessed]          BIT              NULL,
    [PaymentTransactionID] VARCHAR (50)     NULL,
    [CardHolderPhone]      VARCHAR (25)     NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_Orders_ApplicationUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[ApplicationUsers] ([UserID])
);

