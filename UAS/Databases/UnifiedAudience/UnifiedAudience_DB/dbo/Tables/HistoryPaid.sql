CREATE TABLE [dbo].[HistoryPaid] (
    [HistoryPaidID]      INT             IDENTITY (1, 1) NOT NULL,
    [SubscriptionPaidID] INT             NOT NULL,
    [PubSubscriptionID]     INT             NOT NULL,
    [PriceCodeID]        INT             NOT NULL,
    [StartIssueDate]     DATE            NOT NULL,
    [ExpireIssueDate]    DATE            NOT NULL,
    [CPRate]             DECIMAL (10, 2) NULL,
    [Amount]             DECIMAL (10, 2) NULL,
    [AmountPaid]         DECIMAL (10, 2) NULL,
    [BalanceDue]         DECIMAL (10, 2) NULL,
    [PaidDate]           DATETIME        NULL,
    [TotalIssues]        INT             NOT NULL,
    [CheckNumber]        CHAR (20)       NULL,
    [CCNumber]           CHAR (16)       NULL,
    [CCExpirationMonth]  CHAR (2)        NULL,
    [CCEXpirationYear]   CHAR (4)        NULL,
    [CCHolderName]       VARCHAR (100)   NULL,
    [CreditCardTypeID]   INT             NULL,
    [PaymentTypeID]      INT             NOT NULL,
    [DateCreated]        DATETIME        NOT NULL,
    [CreatedByUserID]    INT             NOT NULL,
    CONSTRAINT [PK_HistoryPaid] PRIMARY KEY CLUSTERED ([HistoryPaidID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IX_HistoryPaid_PubSubscriptionID]
    ON [dbo].[HistoryPaid]([PubSubscriptionID] ASC);
GO

