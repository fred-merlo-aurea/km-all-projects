CREATE TABLE [dbo].[ApplicationCode] (
    [AppCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [CodeType]  VARCHAR (50) NOT NULL,
    [CodeValue] VARCHAR (50) NOT NULL,
    [IsDeleted] BIT          CONSTRAINT [DF_ApplicationCode_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ApplicationCode] PRIMARY KEY CLUSTERED ([AppCodeID] ASC) WITH (FILLFACTOR = 80)
);

