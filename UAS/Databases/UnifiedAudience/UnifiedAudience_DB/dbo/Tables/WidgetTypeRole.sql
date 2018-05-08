CREATE TABLE [dbo].[WidgetTypeRole] (
    [TypeID]  UNIQUEIDENTIFIER NOT NULL,
    [Role]    NVARCHAR (50)    NOT NULL,
    [CanEdit] BIT              NOT NULL,
    [CanView] BIT              NOT NULL,
    CONSTRAINT [PK_WidgetTypeRole] PRIMARY KEY CLUSTERED ([TypeID] ASC, [Role] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_WidgetTypeRole_WidgetTypeRole] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[WidgetType] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

