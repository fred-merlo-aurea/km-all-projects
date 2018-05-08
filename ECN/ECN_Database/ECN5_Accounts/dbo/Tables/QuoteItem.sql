CREATE TABLE [dbo].[QuoteItem] (
    [QuoteItemID]        INT            IDENTITY (1, 1) NOT NULL,
    [QuoteId]            INT            NOT NULL,
    [Code]               VARCHAR (50)   NOT NULL,
    [Name]               VARCHAR (100)  NOT NULL,
    [Description]        VARCHAR (500)  NOT NULL,
    [Quantity]           INT            NOT NULL,
    [Rate]               FLOAT (53)     NOT NULL,
    [DiscountRate]       FLOAT (53)     CONSTRAINT [DF_QuoteItems_DiscountRate] DEFAULT ((0)) NULL,
    [LicenseType]        SMALLINT       NOT NULL,
    [PriceType]          SMALLINT       NOT NULL,
    [Frequencytype]      SMALLINT       NOT NULL,
    [IsCustomerCredit]   BIT            CONSTRAINT [DF_QuoteItems_IsCustomerCredit] DEFAULT ((0)) NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF_QuoteItems_IsActive] DEFAULT ((1)) NOT NULL,
    [ProductIDs]         VARCHAR (47)   NULL,
    [ProductFeatureIDs]  VARCHAR (399)  NULL,
    [Services]           VARCHAR (1000) NULL,
    [RecurringProfileID] VARCHAR (100)  CONSTRAINT [DF_QuoteItems_RecurringProfileID] DEFAULT ('') NOT NULL,
    [IsDeleted]          BIT            CONSTRAINT [DF_QuoteItem_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedUserID]      INT            NULL,
    [CreatedDate]        DATETIME       CONSTRAINT [DF_QuoteItem_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]      INT            NULL,
    [UpdatedDate]        DATETIME       NULL,
    CONSTRAINT [PK_QuoteItems] PRIMARY KEY CLUSTERED ([QuoteItemID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_QuoteItems_QuoteId_Code]
    ON [dbo].[QuoteItem]([QuoteId] ASC, [Code] ASC) WITH (FILLFACTOR = 80);

