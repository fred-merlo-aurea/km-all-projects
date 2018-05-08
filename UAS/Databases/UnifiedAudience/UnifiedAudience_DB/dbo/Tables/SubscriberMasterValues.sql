CREATE TABLE [dbo].[SubscriberMasterValues] (
    [MasterGroupID]         INT            NOT NULL,
    [SubscriptionID]        INT            NOT NULL,
    [MastercodesheetValues] VARCHAR (8000) NOT NULL,
    CONSTRAINT [PK_SubscriberMasterValues] PRIMARY KEY CLUSTERED ([MasterGroupID] ASC, [SubscriptionID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriberMasterValues_MasterGroups] FOREIGN KEY ([MasterGroupID]) REFERENCES [dbo].[MasterGroups] ([MasterGroupID]),
    CONSTRAINT [FK_SubscriberMasterValues_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
);
GO
CREATE NONCLUSTERED INDEX [IDX_SubscriberMasterValues_SubscriptionID_MasterGroupID]
    ON [dbo].[SubscriberMasterValues]([SubscriptionID] ASC)
    INCLUDE([MasterGroupID], [MastercodesheetValues]) WITH (FILLFACTOR = 90);
GO

