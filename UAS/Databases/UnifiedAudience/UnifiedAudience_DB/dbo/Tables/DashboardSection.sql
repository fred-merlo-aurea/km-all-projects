CREATE TABLE [dbo].[DashboardSection] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [DashboardID]   UNIQUEIDENTIFIER NOT NULL,
    [RowOrder]      INT              NOT NULL,
    [PanelSettings] NTEXT            NULL,
    [Title]         NVARCHAR (250)   NULL,
    CONSTRAINT [PK_DashboardSection] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DashboardSection_Dashboard] FOREIGN KEY ([DashboardID]) REFERENCES [dbo].[Dashboard] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IDX_DashboardSection_DashboardID_RowOrder]
    ON [dbo].[DashboardSection]([DashboardID] ASC, [RowOrder] ASC) WITH (FILLFACTOR = 90);
GO

