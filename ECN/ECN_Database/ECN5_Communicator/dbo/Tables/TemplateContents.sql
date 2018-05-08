CREATE TABLE [dbo].[TemplateContents] (
    [TemplateContentID] INT           IDENTITY (1, 1) NOT NULL,
    [TemplateID]        INT           NULL,
    [ContentID]         INT           NULL,
    [SlotNumber]        INT           NULL,
    [TemplateSubject]   VARCHAR (255) NULL,
    CONSTRAINT [PK_TemplateContents] PRIMARY KEY CLUSTERED ([TemplateContentID] ASC) WITH (FILLFACTOR = 80)
);

