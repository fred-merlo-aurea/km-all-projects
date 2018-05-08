CREATE TABLE [dbo].[CodeType] (
    [CodeTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID] INT          NOT NULL,
    [CodeType]   VARCHAR (50) CONSTRAINT [DF_CodeType_CodeType] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_CodeType] PRIMARY KEY CLUSTERED ([CodeTypeID] ASC) WITH (FILLFACTOR = 80)
);

