CREATE TABLE [dbo].[BlastFieldsValue] (
    [BlastFieldsValueID] INT           IDENTITY (1, 1) NOT NULL,
    [BlastFieldID]       INT           NULL,
    [CustomerID]         INT           NULL,
    [Value]              VARCHAR (200) NULL,
    [CreatedUserID]      INT           NULL,
    [UpdatedUserID]      INT           NULL,
    [CreatedDate]        DATETIME      NULL,
    [UpdatedDate]        DATETIME      NULL,
    [IsDeleted]          BIT           NULL,
    CONSTRAINT [PK_BlastFieldsValue] PRIMARY KEY CLUSTERED ([BlastFieldsValueID] ASC) WITH (FILLFACTOR = 80)
);

