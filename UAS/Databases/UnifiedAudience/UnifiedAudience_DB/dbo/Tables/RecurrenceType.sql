CREATE TABLE [dbo].[RecurrenceType] (
    [RecurrenceTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [Type]             VARCHAR (50) NULL,
    CONSTRAINT [PK_RecurrenceType] PRIMARY KEY CLUSTERED ([RecurrenceTypeID] ASC) WITH (FILLFACTOR = 90)
);

