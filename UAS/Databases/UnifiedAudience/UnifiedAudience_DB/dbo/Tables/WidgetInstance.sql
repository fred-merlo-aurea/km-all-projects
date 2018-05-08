CREATE TABLE [dbo].[WidgetInstance] (
    [ID]                  UNIQUEIDENTIFIER NOT NULL,
    [TypeID]              UNIQUEIDENTIFIER NOT NULL,
    [Title]               NVARCHAR (100)   NOT NULL,
    [ContainerKey]        NVARCHAR (50)    NOT NULL,
    [ColumnKey]           NVARCHAR (50)    NOT NULL,
    [Position]            INT              NOT NULL,
    [Username]            NVARCHAR (50)    NOT NULL,
    [UserData]            VARBINARY (MAX)  NULL,
    [Closable]            BIT              NOT NULL,
    [Editable]            BIT              NOT NULL,
    [Refreshable]         BIT              NOT NULL,
    [AutoRefreshInterval] INT              CONSTRAINT [DF_WidgetInstance_AutoRefreshInterval_1] DEFAULT ((-1)) NOT NULL,
    [UserSettings]        NTEXT            NULL,
    [Collapsed]           BIT              CONSTRAINT [DF_WidgetInstance_Collapsed] DEFAULT ((0)) NOT NULL,
    [TitleEditable]       BIT              CONSTRAINT [DF_WidgetInstance_TitleEditable] DEFAULT ((1)) NOT NULL,
    [ConfirmClose]        BIT              CONSTRAINT [DF_WidgetInstance_ConfirmClose] DEFAULT ((1)) NOT NULL,
    [SaveCollapsed]       BIT              CONSTRAINT [DF_WidgetInstance_SaveCollapsed] DEFAULT ((0)) NOT NULL,
    [ResizeSettings]      NTEXT            NULL,
    [Description]         NTEXT            NULL,
    [PanelSettings]       NTEXT            NULL,
    [RowKey]              NVARCHAR (50)    NULL,
    [Properties]          NTEXT            NULL,
    CONSTRAINT [PK_WidgetInstance] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_WidgetInstance_WidgetType] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[WidgetType] ([ID]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IDX_WidgetInstance_UserName_ContainerKey]
    ON [dbo].[WidgetInstance]([Username] ASC, [ContainerKey] ASC) WITH (FILLFACTOR = 70);
GO
CREATE NONCLUSTERED INDEX [IDX_WidgetInstance_ContainerKey]
    ON [dbo].[WidgetInstance]([ContainerKey] ASC) WITH (FILLFACTOR = 70);
GO
CREATE NONCLUSTERED INDEX [IDX_WidgetType_GroupDisplayOrder_GroupName_DisplayOrder]
    ON [dbo].[WidgetType]([GroupDisplayOrder] ASC, [GroupName] ASC, [DisplayOrder] ASC) WITH (FILLFACTOR = 70);
GO

