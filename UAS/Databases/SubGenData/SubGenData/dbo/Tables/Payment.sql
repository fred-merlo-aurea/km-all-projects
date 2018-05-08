CREATE TABLE [dbo].[Payment] (
    [account_id]     INT          NOT NULL,
    [amount]         FLOAT (53)   NULL,
    [notes]          VARCHAR (15) NULL,
    [transaction_id] VARCHAR (15) NULL,
    [type]           VARCHAR (50) NULL,
	[order_id]       INT		  NOT NULL,
	[subscriber_id]  INT          NULL,
	[subscription_id] INT         NULL,
	date_created  datetime        NULL, 
	STRecordIdentifier UniqueIdentifier null,
	[bundle_id]   INT            NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY ([order_id])
);

