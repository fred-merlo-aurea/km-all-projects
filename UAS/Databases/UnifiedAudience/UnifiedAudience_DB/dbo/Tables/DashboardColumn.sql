CREATE TABLE [dbo].[DashboardColumn] (
    [DashboardSectionID] UNIQUEIDENTIFIER NOT NULL,
    [ColumnOrder]        INT              NOT NULL,
    [ColumnWidth]        INT              NOT NULL,
    [Padding]            INT              NULL,
    [StyleSpec]          NTEXT            NULL,
    [ColumnSettings]     NTEXT            NULL,
    CONSTRAINT [PK_DashboardColumn] PRIMARY KEY CLUSTERED ([DashboardSectionID] ASC, [ColumnOrder] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DashboardColumn_Dashboard] FOREIGN KEY ([DashboardSectionID]) REFERENCES [dbo].[DashboardSection] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

