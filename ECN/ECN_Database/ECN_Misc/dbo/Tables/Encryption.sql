CREATE TABLE [dbo].[Encryption] (
    [ID]                 INT           IDENTITY (1, 1) NOT NULL,
    [PassPhrase]         NVARCHAR (64) NOT NULL,
    [SaltValue]          NVARCHAR (64) NOT NULL,
    [HashAlgorithm]      NVARCHAR (50) NOT NULL,
    [PasswordIterations] INT           NOT NULL,
    [InitVector]         CHAR (16)     NOT NULL,
    [KeySize]            INT           NOT NULL,
    CONSTRAINT [PK_Encryption] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

