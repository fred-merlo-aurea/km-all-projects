CREATE TABLE [dbo].[NEBook_Customers] (
    [CustomerID]  INT NOT NULL,
    [RegionID]    INT NOT NULL,
    [Goals]       INT NOT NULL,
    [SendCoupons] BIT CONSTRAINT [DF_NEBook_Customers_SendCoupons] DEFAULT (0) NULL
);

