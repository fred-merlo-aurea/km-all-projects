CREATE TABLE [dbo].[PriceCode] (
    [PriceCodeID]     INT             IDENTITY (1, 1) NOT NULL,
	[PublicationID]   INT             NOT NULL,
    [PriceCodes]      VARCHAR (50)    NULL,
	[Term]            INT             NULL,
	[US_CopyRate]	  DECIMAL(18,2)   NULL,
	[CAN_CopyRate]	  DECIMAL(18,2)   NULL,
	[FOR_CopyRate]	  DECIMAL(18,2)   NULL,
    [US_Price]        DECIMAL (18, 2) NULL,
	[CAN_Price]       DECIMAL (18, 2) NULL,
	[FOR_Price]       DECIMAL (18, 2) NULL,
	[QFOfferCode]	  VARCHAR(256)	  NULL,
	[FoxProPriceCode] VARCHAR(256)	  NULL,
	[Description]	  VARCHAR(256)	  NULL,
	[DeliverabilityID]INT			  NULL,
	[TotalIssues]	  INT             DEFAULT ((0)) NULL,
    [IsActive]        BIT             NOT NULL,
    [DateCreated]     DATETIME        NOT NULL,
    [DateUpdated]     DATETIME        NULL,
    [CreatedByUserID] INT             NOT NULL,
    [UpdatedByUserID] INT             NULL
    CONSTRAINT [PK_PriceCode] PRIMARY KEY CLUSTERED ([PriceCodeID] ASC) WITH (FILLFACTOR = 80)
);
