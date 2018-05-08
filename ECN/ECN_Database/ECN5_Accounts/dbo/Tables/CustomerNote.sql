CREATE TABLE [dbo].[CustomerNote] (
    [NoteID]         INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]     INT            NOT NULL,
    [Notes]          VARCHAR (2000) NOT NULL,
    [IsBillingNotes] BIT            CONSTRAINT [DF_CustomerNotes_IsBillingNotes] DEFAULT ((0)) NULL,
    [CreatedBy]      VARCHAR (50)   NULL,
    [UpdatedBy]      VARCHAR (50)   NULL,
    [CreatedDate]    DATETIME       CONSTRAINT [DF_CustomerNotes_DateAdded] DEFAULT (getdate()) NULL,
    [UpdatedDate]    DATETIME       NULL,
    [CreatedUserID]  INT            NULL,
    [UpdatedUserID]  INT            NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_CustomerNote_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CustomerNote] PRIMARY KEY CLUSTERED ([NoteID] ASC) WITH (FILLFACTOR = 80)
);

