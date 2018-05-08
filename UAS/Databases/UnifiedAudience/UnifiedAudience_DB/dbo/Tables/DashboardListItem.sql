CREATE TABLE [dbo].[DashboardListItem] (
    [ListID]       UNIQUEIDENTIFIER NOT NULL,
    [DashboardID]  UNIQUEIDENTIFIER NOT NULL,
    [DisplayOrder] INT              NULL,
    CONSTRAINT [PK_DashboardListItem] PRIMARY KEY CLUSTERED ([ListID] ASC, [DashboardID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DashboardListItem_Dashboard] FOREIGN KEY ([DashboardID]) REFERENCES [dbo].[Dashboard] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_DashboardListItem_DashboardList] FOREIGN KEY ([ListID]) REFERENCES [dbo].[DashboardList] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

