CREATE TABLE [dbo].[Menus] (
    [MenuID]       INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]   INT           NULL,
    [MenuCode]     VARCHAR (50)  NULL,
    [MenuName]     VARCHAR (50)  NULL,
    [MenuTarget]   VARCHAR (255) NULL,
    [SortOrder]    INT           NULL,
    [MenuTypeCode] VARCHAR (50)  NULL,
    [ParentID]     INT           NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED ([MenuID] ASC) WITH (FILLFACTOR = 80)
);

