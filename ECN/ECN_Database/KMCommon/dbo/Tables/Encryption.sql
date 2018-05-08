CREATE TABLE [dbo].[Encryption] (
    [ID]                 INT           IDENTITY (1, 1) NOT NULL,
    [PassPhrase]         NVARCHAR (64) NOT NULL,
    [SaltValue]          NVARCHAR (64) NOT NULL,
    [HashAlgorithm]      NVARCHAR (50) NOT NULL,
    [PasswordIterations] INT           NOT NULL,
    [InitVector]         CHAR (16)     NOT NULL,
    [KeySize]            INT           NOT NULL,
    [IsCurrent]          BIT           NOT NULL,
    [IsActive]           BIT           NOT NULL,
    CONSTRAINT [PK_Encryption] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);



