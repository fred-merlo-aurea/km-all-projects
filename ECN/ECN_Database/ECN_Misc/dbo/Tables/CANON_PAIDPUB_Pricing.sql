CREATE TABLE [dbo].[CANON_PAIDPUB_Pricing] (
    [PriceID]           INT             IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (100)   NOT NULL,
    [Description]       VARCHAR (255)   NULL,
    [ComboFor]          INT             NOT NULL,
    [CustomerID]        INT             NOT NULL,
    [WithNewsletter]    INT             NULL,
    [WithOutNewsletter] INT             NULL,
    [RegularRate1yr]    DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_RegularRate1yr] DEFAULT (0) NOT NULL,
    [ActualRate1yr]     DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_ActualRate1yr] DEFAULT (0) NOT NULL,
    [RegularRate2yr]    DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_RegularRate2yr] DEFAULT (0) NOT NULL,
    [ActualRate2yr]     DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_ActualRate2yr] DEFAULT (0) NOT NULL,
    [RegularRate3yr]    DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_RegularRate3yr] DEFAULT (0) NOT NULL,
    [ActualRate3yr]     DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_ActualRate3yr] DEFAULT (0) NOT NULL,
    [Addldiscount]      DECIMAL (10, 2) CONSTRAINT [DF_CANON_PAIDPUB_Pricing_Addldiscount] DEFAULT (0) NOT NULL,
    [CreatedDate]       DATETIME        CONSTRAINT [DF_CANON_PAIDPUB_Pricing_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]       DATETIME        CONSTRAINT [DF_CANON_PAIDPUB_Pricing_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CANON_PAIDPUB_Pricing] PRIMARY KEY CLUSTERED ([PriceID] ASC) WITH (FILLFACTOR = 80)
);

