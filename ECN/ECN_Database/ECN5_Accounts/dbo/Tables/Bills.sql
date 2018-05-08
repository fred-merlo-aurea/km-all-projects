CREATE TABLE [dbo].[Bills] (
    [BillID]        INT      IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT      NOT NULL,
    [QuoteID]       INT      NOT NULL,
    [CreatedDate]   DATETIME CONSTRAINT [DF_Bills_CREATEDDATE] DEFAULT (getdate()) NOT NULL,
    [Source]        TINYINT  NOT NULL,
    [IsSyncToQB]    BIT      CONSTRAINT [DF_Bills_IsSyncToQB] DEFAULT (0) NOT NULL,
    [CreatedUserID] INT      NULL,
    [IsDeleted]     BIT      CONSTRAINT [DF_Bills_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME NULL,
    [UpdatedUserID] INT      NULL,
    CONSTRAINT [PK_Bills] PRIMARY KEY CLUSTERED ([BillID] ASC) WITH (FILLFACTOR = 80)
);

