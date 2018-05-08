CREATE TABLE [dbo].[Creator_Templates] (
    [TemplateID]   INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]   INT          NULL,
    [TemplateName] VARCHAR (50) NULL,
    [SourceCode]   TEXT         NULL,
    [HeaderCode]   TEXT         NULL,
    CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED ([TemplateID] ASC) WITH (FILLFACTOR = 80)
);

