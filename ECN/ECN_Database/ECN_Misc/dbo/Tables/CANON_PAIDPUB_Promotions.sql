CREATE TABLE [dbo].[CANON_PAIDPUB_Promotions] (
    [PromotionID] INT             IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)    NOT NULL,
    [Description] VARCHAR (200)   NULL,
    [CustomerID]  INT             NULL,
    [Code]        VARCHAR (10)    NOT NULL,
    [Discount]    DECIMAL (10, 2) CONSTRAINT [DF_CANON_PaidPub_Promotions_Discount] DEFAULT (0) NULL,
    [IsActive]    BIT             CONSTRAINT [DF_CANON_PaidPub_Promotions_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CANON_PaidPub_Promotions] PRIMARY KEY CLUSTERED ([PromotionID] ASC) WITH (FILLFACTOR = 80)
);

