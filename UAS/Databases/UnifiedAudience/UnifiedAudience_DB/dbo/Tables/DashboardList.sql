CREATE TABLE [dbo].[DashboardList] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [Username]   NVARCHAR (50)    NOT NULL,
    [Title]      NVARCHAR (250)   NULL,
    [CreateDate] DATETIME         NOT NULL,
    CONSTRAINT [PK_DashboardList] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE NONCLUSTERED INDEX [IDX_DashboardList_Username_CreateDate]
    ON [dbo].[DashboardList]([Username] ASC, [CreateDate] ASC) WITH (FILLFACTOR = 90);
GO