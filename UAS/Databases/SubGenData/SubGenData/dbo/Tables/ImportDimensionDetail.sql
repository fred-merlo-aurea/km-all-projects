CREATE TABLE [dbo].[ImportDimensionDetail] (
    [ImportDimensionId]  INT           NOT NULL,
    [DimensionField]     VARCHAR (250) NOT NULL,
    [DimensionValue]     VARCHAR (500) NOT NULL,
    [SystemSubscriberID] INT           NOT NULL,
    [SubscriptionID]     INT           NOT NULL,
    [PublicationID]      INT           DEFAULT ((0)) NOT NULL
);


