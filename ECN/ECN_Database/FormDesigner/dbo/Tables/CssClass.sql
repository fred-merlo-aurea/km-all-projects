CREATE TABLE [dbo].[CssClass] (
    [CssClass_Seq_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CssClass] PRIMARY KEY CLUSTERED ([CssClass_Seq_ID] ASC)
);

