CREATE TABLE [dbo].[CANON_PAIDPUB_eNewsletter_Category] (
    [CategoryID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID] INT           NOT NULL,
    [Name]       VARCHAR (50)  NOT NULL,
    [Desc]       VARCHAR (200) NULL,
    CONSTRAINT [PK_CANON_PAIDPIB_eNewsletter_Category] PRIMARY KEY CLUSTERED ([CategoryID] ASC) WITH (FILLFACTOR = 80)
);

