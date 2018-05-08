CREATE TABLE [dbo].[GCCanada] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [ContactName] VARCHAR (255) NULL,
    [Address]     VARCHAR (50)  NULL,
    [Postal]      VARCHAR (255) NULL,
    [Phone]       VARCHAR (50)  NULL,
    [Email]       VARCHAR (255) NULL,
    CONSTRAINT [PK_GCCanada] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

