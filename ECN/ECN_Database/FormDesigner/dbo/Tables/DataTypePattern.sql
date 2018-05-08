CREATE TABLE [dbo].[DataTypePattern] (
    [DataTypePattern_Seq_ID] INT            NOT NULL,
    [DataTypeName]           NVARCHAR (30)  NOT NULL,
    [Pattern]                NVARCHAR (400) NOT NULL,
    CONSTRAINT [PK_DataTypePattern] PRIMARY KEY CLUSTERED ([DataTypePattern_Seq_ID] ASC)
);

