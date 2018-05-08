CREATE TABLE [dbo].[SubscriptionStatusMatrix] (
    [StatusMatrixID]       INT      IDENTITY (1, 1) NOT NULL,
    [SubscriptionStatusID] INT      NOT NULL,
    [CategoryCodeID]       INT      NOT NULL,
    [TransactionCodeID]    INT      NOT NULL,
    [IsActive]             BIT      NOT NULL,
    [DateCreated]          DATETIME NOT NULL,
    [DateUpdated]          DATETIME NULL,
    [CreatedByUserID]      INT      NOT NULL,
    [UpdatedByUserID]      INT      NULL,
    CONSTRAINT [PK_SubscriptionStatusMatrix] PRIMARY KEY CLUSTERED ([SubscriptionStatusID] ASC, [CategoryCodeID] ASC, [TransactionCodeID] ASC) WITH (FILLFACTOR = 80)
);

