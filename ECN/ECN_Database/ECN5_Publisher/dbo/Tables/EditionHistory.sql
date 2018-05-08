CREATE TABLE [dbo].[EditionHistory] (
    [EditionHistoryID] INT      IDENTITY (1, 1) NOT NULL,
    [EditionID]        INT      NOT NULL,
    [ActivatedDate]    DATETIME NULL,
    [ArchievedDate]    DATETIME NULL,
    [DeActivatedDate]  DATETIME NULL,
    [CreatedUserID]    INT      NULL,
    [CreatedDate]      DATETIME CONSTRAINT [DF_EditionHistory_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]    INT      NULL,
    [UpdatedDate]      DATETIME NULL,
    [IsDeleted]        BIT      CONSTRAINT [DF_EditionHistory_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EditionHistory] PRIMARY KEY CLUSTERED ([EditionHistoryID] ASC) WITH (FILLFACTOR = 80)
);

