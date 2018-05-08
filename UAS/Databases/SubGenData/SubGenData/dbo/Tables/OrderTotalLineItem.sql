CREATE TABLE [dbo].[OrderTotalLineItem] (
    [order_id]    INT        NOT NULL,
    [account_id]  INT        NOT NULL,
    [product_id]  INT        NULL,
    [bundle_id]   INT        NULL,
    [price]       FLOAT (53) NULL,
    [sub_total]   FLOAT (53) NULL,
    [tax_total]   FLOAT (53) NULL,
    [grand_total] FLOAT (53) NULL
);

