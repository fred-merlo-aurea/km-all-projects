CREATE TABLE [dbo].[SubscriptionList] (
    [subscription_id] INT          NOT NULL,
    [account_id]      INT          NOT NULL,
    [copies]          INT          NULL,
    [is_grace]        BIT          NULL,
    [is_paid]         BIT          NULL,
    [type]            VARCHAR (50) NULL
);

