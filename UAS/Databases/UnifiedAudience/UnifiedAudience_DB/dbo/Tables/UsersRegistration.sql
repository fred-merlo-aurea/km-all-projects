﻿CREATE TABLE [dbo].[UsersRegistration] (
    [UserID]               UNIQUEIDENTIFIER NULL,
    [Company]              VARCHAR (45)     NULL,
    [Fname]                VARCHAR (20)     NULL,
    [Lname]                VARCHAR (25)     NULL,
    [Phone]                VARCHAR (22)     NULL,
    [Address1]             VARCHAR (45)     NULL,
    [Address2]             VARCHAR (45)     NULL,
    [City]                 VARCHAR (30)     NULL,
    [State]                VARCHAR (2)      NULL,
    [Zip]                  VARCHAR (5)      NULL,
    [CountryID]            INT              NULL,
    [CardHolderName]       VARCHAR (50)     NULL,
    [CardType]             VARCHAR (25)     NULL,
    [CardNo]               VARCHAR (4)      NULL,
    [CardExpirationMonth]  INT              NULL,
    [CardExpirationYear]   INT              NULL,
    [CardCVV]              VARCHAR (10)     NULL,
    [RegID]                INT              IDENTITY (1, 1) NOT NULL,
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
    [TrailtoPaid]          BIT              NULL,
    CONSTRAINT [PK_UsersRegistration] PRIMARY KEY CLUSTERED ([RegID] ASC)
);

