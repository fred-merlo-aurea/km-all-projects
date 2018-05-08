CREATE TABLE [dbo].[CssFile] (
    [CssFile_Seq_ID] INT              IDENTITY (1, 1) NOT NULL,
    [CssFileUID]     UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_CssFile] PRIMARY KEY CLUSTERED ([CssFile_Seq_ID] ASC)
);

