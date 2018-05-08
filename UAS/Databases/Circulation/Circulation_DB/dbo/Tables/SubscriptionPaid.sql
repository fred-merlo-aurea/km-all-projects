CREATE TABLE [dbo].[SubscriptionPaid] (
    [SubscriptionPaidID] INT             IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]     INT             NOT NULL,
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
    [CCExpirationYear]   CHAR (4)        NULL,
    [CCHolderName]       VARCHAR (100)   NULL,
    [CreditCardTypeID]   INT             NULL,
    [PaymentTypeID]      INT             NOT NULL,
	[DeliverID]          INT             NULL,
    [GraceIssues]		 INT			 NULL, 
	[WriteOffAmount]	 DECIMAL(10,2)   NULL,
	[OtherType]			 VARCHAR(256)	 NULL,
    [DateCreated]        DATETIME        NOT NULL,
    [DateUpdated]        DATETIME        NULL,
    [CreatedByUserID]    INT             NOT NULL,
    [UpdatedByUserID]    INT             NULL,
	[Frequency]			 INT			 NULL,
	[Term]				 INT			 NULL

    CONSTRAINT [PK_SubscriptionPaid] PRIMARY KEY CLUSTERED ([SubscriptionPaidID] ASC) WITH (FILLFACTOR = 80)
);

