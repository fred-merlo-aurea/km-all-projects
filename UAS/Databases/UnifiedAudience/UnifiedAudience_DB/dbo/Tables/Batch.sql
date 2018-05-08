CREATE TABLE [dbo].[Batch] (
    [BatchID]       INT      IDENTITY (1, 1) NOT NULL,
    [PublicationID] INT      NOT NULL,
    [UserID]        INT      NOT NULL,
    [BatchCount]    INT      CONSTRAINT [DF_Batch_BatchCount] DEFAULT ((0)) NOT NULL,
    [IsActive]      BIT      CONSTRAINT [DF_Batch_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]   DATETIME NOT NULL,
    [DateFinalized] DATETIME NULL,
	[BatchNumber]   INT	     NULL
    CONSTRAINT [PK_Batch] PRIMARY KEY CLUSTERED ([BatchID] ASC) WITH (FILLFACTOR = 80)
);

