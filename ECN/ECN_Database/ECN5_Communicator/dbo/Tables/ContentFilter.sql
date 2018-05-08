CREATE TABLE [dbo].[ContentFilter] (
    [FilterID]      INT          IDENTITY (100, 1) NOT NULL,
    [LayoutID]      INT          NULL,
    [SlotNumber]    INT          NULL,
    [ContentID]     INT          NULL,
    [GroupID]       INT          NULL,
    [FilterName]    VARCHAR (50) NULL,
    [WhereClause]   TEXT         NULL,
    [CreatedDate]   DATETIME     CONSTRAINT [DF_ContentFilter_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_ContentFilter_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    CONSTRAINT [PK_ContentFilters] PRIMARY KEY CLUSTERED ([FilterID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_ContentFilters_GroupID]
    ON [dbo].[ContentFilter]([GroupID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ContentFilter] TO [reader]
    AS [dbo];

