CREATE TABLE [dbo].[BillItems] (
    [BillItemID]                 INT           IDENTITY (1, 1) NOT NULL,
    [BillID]                     INT           NOT NULL,
    [QuoteItemID]                INT           NOT NULL,
    [ChangedDate]                DATETIME      NOT NULL,
    [Quantity]                   INT           NOT NULL,
    [Rate]                       FLOAT (53)    NOT NULL,
    [StartDate]                  DATETIME      NOT NULL,
    [EndDate]                    DATETIME      NOT NULL,
    [Status]                     TINYINT       NOT NULL,
    [TransactionID]              VARCHAR (100) CONSTRAINT [DF_BillItems_TransactionID] DEFAULT ('') NOT NULL,
    [IsProcessedByLicenseEngine] BIT           CONSTRAINT [DF_BillItems_IsProcessedByLicenseEngine] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_BillItems] PRIMARY KEY CLUSTERED ([BillItemID] ASC) WITH (FILLFACTOR = 80)
);

