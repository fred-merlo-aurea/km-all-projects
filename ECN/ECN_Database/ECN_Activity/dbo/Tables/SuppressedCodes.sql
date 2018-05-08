CREATE TABLE [dbo].[SuppressedCodes] (
    [SuppressedCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [SupressedCode]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SuppressedCodes] PRIMARY KEY CLUSTERED ([SuppressedCodeID] ASC) WITH (FILLFACTOR = 80)
);

