CREATE TABLE [dbo].[OrderTotal] (
    [order_id]    INT        NOT NULL,
    [account_id]  INT        NOT NULL,
    [sub_total]   FLOAT (53) NULL,
    [tax_total]   FLOAT (53) NULL,
    [grand_total] FLOAT (53) NULL
);

