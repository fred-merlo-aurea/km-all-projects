CREATE TABLE [dbo].[Purchase] (
    [account_id]         INT          NOT NULL,
    [billing_address_id] INT          NOT NULL,
    [bundle_id]          INT          NULL,
    [invoice_id]         INT          NULL,
    [name]               VARCHAR (50) NULL,
    [subscriber_id]      INT          NULL,
	[IsProcessed]		 BIT DEFAULT('false') NULL,
	[ProcessedDate]		 DATETIME	  NULL
);

