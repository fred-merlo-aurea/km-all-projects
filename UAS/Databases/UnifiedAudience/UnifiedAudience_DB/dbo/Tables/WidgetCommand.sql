CREATE TABLE [dbo].[WidgetCommand] (
    [ID]           UNIQUEIDENTIFIER NOT NULL,
    [TypeID]       UNIQUEIDENTIFIER NOT NULL,
    [CommandName]  NVARCHAR (50)    NOT NULL,
    [Icon]         NVARCHAR (15)    NOT NULL,
    [Hint]         NTEXT            NULL,
    [DisplayOrder] INT              NOT NULL,
    [FunctionName] NVARCHAR (25)    NULL,
    CONSTRAINT [PK_WidgetCommand] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_WidgetCommand_WidgetType] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[WidgetType] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IDX_WidgetCommand_TypeID]
    ON [dbo].[WidgetCommand]([TypeID] ASC) WITH (FILLFACTOR = 70);
GO

