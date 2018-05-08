CREATE TABLE [dbo].[Filter] (
    [FilterID]         INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT          NULL,
    [CreatedUserID]    INT          NULL,
    [GroupID]          INT          NULL,
    [FilterName]       VARCHAR (50) NULL,
    [WhereClause]      TEXT         NULL,
    [DynamicWhere]     TEXT         NULL,
    [CreatedDate]      DATETIME     CONSTRAINT [DF_Filter_CREATEDDATE] DEFAULT (getdate()) NULL,
    [GroupCompareType] VARCHAR (50) NULL,
    [IsDeleted]        BIT          CONSTRAINT [DF_Filter_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]      DATETIME     NULL,
    [UpdatedUserID]    INT          NULL,
    [Archived] BIT NULL, 
    CONSTRAINT [PK_Filters] PRIMARY KEY CLUSTERED ([FilterID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_Filters_CustomerID_GroupID]
    ON [dbo].[Filter]([CustomerID] ASC, [GroupID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Filter] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Filter] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Filter] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Filter] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Filter] TO [reader]
    AS [dbo];

