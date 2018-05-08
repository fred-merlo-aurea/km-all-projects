CREATE TABLE [dbo].[PubSubscriptionDetail] (
    [PubSubscriptionDetailID] INT      IDENTITY (1, 1) NOT NULL,
    [PubSubscriptionID]       INT      NULL,
    [SubscriptionID]          INT      NULL,
    [CodesheetID]             INT      NULL,
    [DateCreated]             DATETIME CONSTRAINT [DF_PubSubscriptionDetail_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]             DATETIME NULL,
    [CreatedByUserID]         INT      NULL,
    [UpdatedByUserID]         INT      NULL,
	[ResponseOther]			  VARCHAR(256) NULL
    CONSTRAINT [PK_PubSubscriptionDetail] PRIMARY KEY CLUSTERED ([PubSubscriptionDetailID] ASC) WITH (FILLFACTOR = 90),
	CONSTRAINT [FK_PubSubscriptionDetail_CodeSheet] FOREIGN KEY ([CodesheetID]) REFERENCES [dbo].[CodeSheet] ([CodeSheetID]),
    CONSTRAINT [FK_PubSubscriptionDetail_PubSubscriptions] FOREIGN KEY ([PubSubscriptionID]) REFERENCES [dbo].[PubSubscriptions] ([PubSubscriptionID]),
    CONSTRAINT [FK_PubSubscriptionDetail_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
);
GO

CREATE NONCLUSTERED INDEX [IDX_PubSubscriptionDetail_CodeSheetID]
    ON [dbo].[PubSubscriptionDetail]([CodesheetID] ASC)
    INCLUDE([PubSubscriptionID]);
GO

CREATE NONCLUSTERED INDEX [idx_PubSubscriptionDetail_PubSubscriptionID_CodesheetID]
    ON [dbo].[PubSubscriptionDetail]([PubSubscriptionID] ASC, [CodesheetID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_PubSubscriptionDetail_SubscriptionID]
    ON [dbo].[PubSubscriptionDetail]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_PubSubscriptionDetail_PubSubscriptionID]
    ON [dbo].[PubSubscriptionDetail]([PubSubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_PubSubscriptionDetail_SubscriptionID_CodesheetID]
     ON [dbo].[PubSubscriptionDetail]([CodesheetID] ASC, [SubscriptionID] ASC)
    INCLUDE([PubSubscriptionID]) WITH (FILLFACTOR = 90);
GO