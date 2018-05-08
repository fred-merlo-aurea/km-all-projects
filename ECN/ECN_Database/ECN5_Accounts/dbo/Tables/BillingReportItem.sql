CREATE TABLE [dbo].[BillingReportItem] (
    [BillingItemID]   INT             NOT NULL IDENTITY,
    [BillingReportID] INT             NOT NULL,
    [BaseChannelID]   INT             NULL,
    [BaseChannelName] VARCHAR (100)    NULL,
    [CustomerID]      INT             NULL,
    [CustomerName]    VARCHAR (100)    NULL,
    [IsFlatRateItem]  BIT             NULL,
    [IsMasterFile]    BIT             NULL,
    [IsFulfillment]   BIT             NULL,
    [Amount]          DECIMAL (18, 2) NULL,
    [AmountOfItems]   INT             NULL,
    [InvoiceText]     VARCHAR (255)   NULL,
    [CreatedDate]     DATETIME        NULL,
    [CreatedUserID]   INT             NULL,
    [UpdatedDate]     DATETIME        NULL,
    [UpdatedUserID]   INT             NULL,
    [IsDeleted]       BIT             NULL,
    CONSTRAINT [PK_BillingReportItem] PRIMARY KEY CLUSTERED ([BillingItemID] ASC)
);

