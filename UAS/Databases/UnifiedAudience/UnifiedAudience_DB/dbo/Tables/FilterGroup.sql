CREATE TABLE [dbo].[FilterGroup] (
    [FilterGroupID] INT IDENTITY (1, 1) NOT NULL,
    [FilterID]      INT NULL,
    [SortOrder]     INT NULL,
    CONSTRAINT [PK_FilterGroup] PRIMARY KEY CLUSTERED ([FilterGroupID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FilterGroup_FilterGroup] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filters] ([FilterID])
);

