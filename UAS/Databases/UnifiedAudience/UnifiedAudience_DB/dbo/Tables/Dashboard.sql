CREATE TABLE [dbo].[Dashboard] (
    [ID]                UNIQUEIDENTIFIER NOT NULL,
    [Title]             NVARCHAR (250)   NOT NULL,
    [Username]          NVARCHAR (50)    NOT NULL,
    [ShareType]         NVARCHAR (25)    NOT NULL,
    [CreateDate]        DATETIME         NOT NULL,
    [Description]       NTEXT            NULL,
    [UserSettings]      NTEXT            NULL,
    [GroupName]         NVARCHAR (250)   NULL,
    [DisplayOrder]      INT              NULL,
    [GroupDisplayOrder] INT              NULL,
    [UserTag]           NTEXT            NULL,
    [PanelSettings]     NTEXT            NULL,
    [Url]               NVARCHAR (250)   NULL,
    [ViewMode]          NVARCHAR (25)    NULL,
    [DashboardSettings] NTEXT            NULL,
    CONSTRAINT [PK_Dashboard] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE NONCLUSTERED INDEX [IDX_Dashboard_Username_GroupDisplayOrder_GroupName_DisplayOrder]
    ON [dbo].[Dashboard]([Username] ASC, [GroupDisplayOrder] ASC, [GroupName] ASC, [DisplayOrder] ASC) WITH (FILLFACTOR = 90);
GO
CREATE NONCLUSTERED INDEX [IDX_Dashboard_GroupDisplayOrder_GroupName_DisplayOrder]
    ON [dbo].[Dashboard]([GroupDisplayOrder] ASC, [GroupName] ASC, [DisplayOrder] ASC) WITH (FILLFACTOR = 90);