CREATE TABLE [dbo].[ImportDimension] (
    [ImportDimensionId]  INT      IDENTITY (1, 1) NOT NULL,
    [SystemSubscriberID] INT      NULL,
    [SubscriptionID]     INT      NULL,
    [PublicationID]      INT      NULL,
    [account_id]         INT      NULL,
    [DateUpdated]        DATETIME NULL,
    [IsMergedToUAD]      BIT      CONSTRAINT [DF__ImportDim__IsMer__5812160E] DEFAULT ('false') NOT NULL,
    [DateMergedToUAD]    DATETIME NULL,
    [DateCreated]        DATETIME CONSTRAINT [DF__ImportDim__DateC__59063A47] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ImportDimension] PRIMARY KEY CLUSTERED ([ImportDimensionId] ASC)
);


GO



