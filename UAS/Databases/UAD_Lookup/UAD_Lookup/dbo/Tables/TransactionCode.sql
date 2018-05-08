CREATE TABLE [dbo].[TransactionCode] (
    [TransactionCodeID]     INT          IDENTITY (1, 1) NOT NULL,
    [TransactionCodeTypeID] INT          NOT NULL,
    [TransactionCodeName]   VARCHAR (50) NOT NULL,
    [TransactionCodeValue]  INT          NULL,
    [IsActive]              BIT          NOT NULL,
    [DateCreated]           DATETIME     NOT NULL,
    [DateUpdated]           DATETIME     NULL,
    [CreatedByUserID]       INT          NOT NULL,
    [UpdatedByUserID]       INT          NULL,
    [IsKill]                BIT          CONSTRAINT [DF_TransactionCode_IsKill] DEFAULT ((0)) NULL
);

