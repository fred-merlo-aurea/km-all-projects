CREATE TABLE [dbo].[OrderItem] (
    [order_item_id]  INT        NOT NULL,
    [account_id]     INT        NOT NULL,
    [bundle_id]      INT        NULL,
    [refund_date]    DATETIME   NULL,
    [fulfilled_date] DATETIME   NULL,
    [sub_total]      FLOAT (53) NULL,
    [tax_total]      FLOAT (53) NULL,
    [grand_total]    FLOAT (53) NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([order_item_id] ASC) WITH (FILLFACTOR = 90)
);

