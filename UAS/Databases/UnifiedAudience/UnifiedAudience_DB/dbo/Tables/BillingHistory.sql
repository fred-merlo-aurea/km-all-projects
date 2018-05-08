CREATE TABLE [dbo].[BillingHistory] (
    [PaymentID]            INT              IDENTITY (1, 1) NOT NULL,
    [UserID]               UNIQUEIDENTIFIER NULL,
    [BillingAddress1]      VARCHAR (45)     NULL,
    [BillingAddress2]      VARCHAR (45)     NULL,
    [BillingCity]          VARCHAR (30)     NULL,
    [BillingState]         VARCHAR (2)      NULL,
    [BillingZip]           VARCHAR (5)      NULL,
    [BillingCountryID]     INT              NULL,
    [IsProcessed]          BIT              NULL,
    [PaymentTransactionID] VARCHAR (50)     NULL,
    [SubscriptionFee]      DECIMAL (10, 2)  NULL,
    [DateAdded]            DATETIME         NULL,
    [CardNo]               VARCHAR (4)      NULL,
    [CardHolderName]       VARCHAR (50)     NULL,
    [CardType]             VARCHAR (25)     NULL,
    [CardExpirationMonth]  INT              NULL,
    [CardExpirationYear]   INT              NULL,
    [CardCVV]              VARCHAR (10)     NULL,
    [phone]                VARCHAR (22)     NULL,
    CONSTRAINT [PK_BillingHistory] PRIMARY KEY CLUSTERED ([PaymentID] ASC),
    CONSTRAINT [FK_BillingHistory_ApplicationUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[ApplicationUsers] ([UserID])
);

