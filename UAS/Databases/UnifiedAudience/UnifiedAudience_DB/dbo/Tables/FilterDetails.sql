CREATE TABLE [dbo].[FilterDetails] (
    [FilterDetailsID] INT           IDENTITY (1, 1) NOT NULL,
    [FilterID]        INT           NULL,
    [FilterType]      INT           NULL,
    [Group]           VARCHAR (100) NULL,
    [Name]            VARCHAR (50)  NULL,
    [Values]          VARCHAR (MAX) NULL,
    [SearchCondition] VARCHAR (50)  NULL,
    [FilterGroupID]   INT           NULL,
    CONSTRAINT [PK_FilterDetails] PRIMARY KEY CLUSTERED ([FilterDetailsID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FilterDetails_FilterDetails] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filters] ([FilterID])
);

