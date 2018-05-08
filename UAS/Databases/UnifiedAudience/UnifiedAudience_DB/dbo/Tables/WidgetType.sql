CREATE TABLE [dbo].[WidgetType] (
    [ID]                  UNIQUEIDENTIFIER NOT NULL,
    [ApplicationName]     NVARCHAR (50)    NULL,
    [Title]               NVARCHAR (100)   NOT NULL,
    [Description]         NVARCHAR (250)   NULL,
    [ControlPath]         NVARCHAR (150)   NOT NULL,
    [EditorPath]          NVARCHAR (150)   NULL,
    [Closable]            BIT              NOT NULL,
    [Editable]            BIT              NOT NULL,
    [Refreshable]         BIT              NOT NULL,
    [EditorSize]          NVARCHAR (15)    NULL,
    [AutoRefreshInterval] INT              CONSTRAINT [DF_WidgetType_AutoRefreshInterval] DEFAULT ((-1)) NOT NULL,
    [GroupName]           NVARCHAR (150)   NULL,
    [DisplayOrder]        INT              NULL,
    [NeedsReloading]      BIT              CONSTRAINT [DF_WidgetType_NeedsReloading] DEFAULT ((0)) NOT NULL,
    [GroupDisplayOrder]   INT              NULL,
    [SaveCollapsed]       BIT              CONSTRAINT [DF_WidgetType_SaveCollapsed] DEFAULT ((0)) NOT NULL,
    [CreateCollapsed]     BIT              CONSTRAINT [DF_WidgetType_CreateCollapsed] DEFAULT ((0)) NOT NULL,
    [TitleEditable]       BIT              CONSTRAINT [DF_WidgetType_TitleEditable] DEFAULT ((1)) NOT NULL,
    [ConfirmClose]        BIT              CONSTRAINT [DF_WidgetType_ConfirmClose] DEFAULT ((1)) NOT NULL,
    [ResizeSettings]      NTEXT            NULL,
    [TypeKey]             NVARCHAR (50)    NULL,
    [PanelSettings]       NTEXT            NULL,
    [WidgetProperties]    NTEXT            NULL,
    CONSTRAINT [PK_WidgetTypes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO


