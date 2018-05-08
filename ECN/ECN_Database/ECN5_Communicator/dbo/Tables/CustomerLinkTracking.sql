CREATE TABLE [dbo].[CustomerLinkTracking] (
    [CLTID]      INT IDENTITY (1, 1) NOT NULL,
    [CustomerID] INT NULL,
    [LTPID]      INT NULL,
    [LTPOID]     INT NULL,
    [IsActive]   BIT NULL,
    CONSTRAINT [PK_CustomerLinkTracking] PRIMARY KEY CLUSTERED ([CLTID] ASC) WITH (FILLFACTOR = 80)
);

