CREATE TABLE [dbo].[CustomerPlans] (
    [PlanID]                 INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]             INT           NOT NULL,
    [QuoteOptionID]          INT           NULL,
    [ActivationDate]         DATETIME      NULL,
    [CardOwnerName]          VARCHAR (100) NULL,
    [CardNumber]             VARCHAR (50)  NULL,
    [CardType]               VARCHAR (25)  NULL,
    [CardExpiration]         VARCHAR (50)  NULL,
    [CardVerificationNumber] VARCHAR (50)  NULL,
    [subscriptionType]       CHAR (1)      NULL,
    [IsPhoneSupportIncluded] BIT           NULL,
    CONSTRAINT [PK_CustomerPlans] PRIMARY KEY CLUSTERED ([PlanID] ASC) WITH (FILLFACTOR = 80)
);

