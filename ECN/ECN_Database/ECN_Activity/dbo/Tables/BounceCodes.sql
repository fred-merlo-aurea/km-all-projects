CREATE TABLE [dbo].[BounceCodes] (
    [BounceCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [BounceCode]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BounceCodes] PRIMARY KEY CLUSTERED ([BounceCodeID] ASC) WITH (FILLFACTOR = 80)
);

