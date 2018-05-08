CREATE TABLE [dbo].[SubscriptionResponseMap] (
    [SubscriptionID]  INT           NOT NULL,
    [ResponseID]      INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    [ResponseOther]   VARCHAR (300) DEFAULT ('') NULL,
    CONSTRAINT [PK_SubscriptionResponseMap] PRIMARY KEY CLUSTERED ([SubscriptionID] ASC, ResponseID ASC) WITH (FILLFACTOR = 80)
);

