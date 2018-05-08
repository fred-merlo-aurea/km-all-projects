CREATE TABLE [dbo].[SubscriptionDetails] (
    [sdID]           INT IDENTITY (1, 1) NOT NULL,
    [SubscriptionID] INT NOT NULL,
    [MasterID]       INT NOT NULL,
    CONSTRAINT [PK_SubscriptionDetails] PRIMARY KEY CLUSTERED ([sdID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriptionDetails_Mastercodesheet] FOREIGN KEY ([MasterID]) REFERENCES [dbo].[Mastercodesheet] ([MasterID]),
    CONSTRAINT [FK_SubscriptionDetails_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_SubscriptionDetails_SubscriptionID_MasterID]
    ON [dbo].[SubscriptionDetails]([SubscriptionID] ASC, [MasterID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriptionDetails_MasterID] 
	ON [dbo].[SubscriptionDetails] ([MasterID]) INCLUDE ([SubscriptionID]) WITH (FILLFACTOR = 90);
GO
