CREATE TABLE [dbo].[Order] (
    [order_id]           INT          NOT NULL,
    [account_id]         INT          NOT NULL,
    [mailing_address_id] INT          NULL,
    [billing_address_id] INT          NULL,
    [subscriber_id]      INT          NULL,
    [import_name]        VARCHAR (30) NULL,
    [user_id]            INT          NULL,
    [channel_id]         INT          NULL,
    [order_date]         DATETIME     NULL,
    [is_gift]            BIT          NULL,
    [sub_total]          FLOAT (53)   NULL,
    [tax_total]          FLOAT (53)   NULL,
    [grand_total]        FLOAT (53)   NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([order_id] ASC) WITH (FILLFACTOR = 90)
);

