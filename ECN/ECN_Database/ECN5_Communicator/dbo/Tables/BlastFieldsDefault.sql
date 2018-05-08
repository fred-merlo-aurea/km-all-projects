CREATE TABLE [dbo].[BlastFieldsDefault] (
    [BlastFieldID] INT           IDENTITY (1, 1) NOT NULL,
    [DefaultName]  VARCHAR (200) NULL,
    CONSTRAINT [PK_BlastFieldsDefault] PRIMARY KEY CLUSTERED ([BlastFieldID] ASC) WITH (FILLFACTOR = 80)
);

