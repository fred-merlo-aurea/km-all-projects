CREATE TABLE [dbo].[TransactionCodeType] (
    [TransactionCodeTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [TransactionCodeTypeName] VARCHAR (50) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_TransactionCodeType_IsActive] DEFAULT ((0)) NOT NULL,
    [IsFree]                  BIT          NULL,
    [DateCreated]             DATETIME     NOT NULL,
    [DateUpdated]             DATETIME     NULL,
    [CreatedByUserID]         INT          NOT NULL,
    [UpdatedByUserID]         INT          NULL,
    CONSTRAINT [PK_TransactionCodeType] PRIMARY KEY CLUSTERED ([TransactionCodeTypeID] ASC) WITH (FILLFACTOR = 80)
);

