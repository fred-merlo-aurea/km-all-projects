CREATE FULLTEXT INDEX ON [dbo].[Subscription]
    KEY INDEX [IDX_SubscriptionID]
    ON [Subscriber Catalog];


GO
ALTER FULLTEXT INDEX ON [dbo].[Subscription] DISABLE;

