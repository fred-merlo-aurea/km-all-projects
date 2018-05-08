CREATE TABLE [dbo].[BlastFieldsName] (
    [BlastFieldsNameID] INT           IDENTITY (1, 1) NOT NULL,
    [BlastFieldID]      INT           NULL,
    [CustomerID]        INT           NULL,
    [Name]              VARCHAR (200) NULL,
    [CreatedUserID]     INT           NULL,
    [CreatedDate]       DATETIME      NULL,
    [UpdatedUserID]     INT           NULL,
    [UpdatedDate]       DATETIME      NULL,
    [IsDeleted]         BIT           NULL,
    CONSTRAINT [PK_BlastFieldsName] PRIMARY KEY CLUSTERED ([BlastFieldsNameID] ASC) WITH (FILLFACTOR = 80)
);

