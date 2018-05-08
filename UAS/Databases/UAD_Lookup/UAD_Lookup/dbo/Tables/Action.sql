CREATE TABLE [dbo].[Action] (
    [ActionID]          INT            IDENTITY (1, 1) NOT NULL,
    [ActionTypeID]      INT            NOT NULL,
    [CategoryCodeID]    INT            NOT NULL,
    [TransactionCodeID] INT            NOT NULL,
    [Note]              NVARCHAR (MAX) NULL,
    [IsActive]          BIT            CONSTRAINT [DF_Action_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]       DATETIME       NULL,
    [DateUpdated]       DATETIME       NULL,
    [CreatedByUserID]   INT            NULL,
    [UpdatedByUserID]   INT            NULL,
    CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED ([ActionTypeID] ASC, [CategoryCodeID] ASC, [TransactionCodeID] ASC) WITH (FILLFACTOR = 80)
);

