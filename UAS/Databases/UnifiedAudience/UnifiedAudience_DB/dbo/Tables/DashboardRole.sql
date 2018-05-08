CREATE TABLE [dbo].[DashboardRole] (
    [DashboardID] UNIQUEIDENTIFIER NOT NULL,
    [Role]        NVARCHAR (50)    NOT NULL,
    [CanEdit]     BIT              NOT NULL,
    [CanView]     BIT              NOT NULL,
    CONSTRAINT [PK_DashboardRole] PRIMARY KEY CLUSTERED ([DashboardID] ASC, [Role] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DashboardRole_DashboardRole] FOREIGN KEY ([DashboardID]) REFERENCES [dbo].[Dashboard] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IDX_DashboardRole_Role]
    ON [dbo].[DashboardRole]([Role] ASC) WITH (FILLFACTOR = 90);
GO

