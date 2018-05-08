CREATE TABLE [dbo].[FiltersDetails] (
    [FDID]         INT           IDENTITY (1, 1) NOT NULL,
    [FilterID]     INT           NULL,
    [FieldType]    VARCHAR (50)  NULL,
    [CompareType]  VARCHAR (50)  NULL,
    [FieldName]    VARCHAR (100) NULL,
    [Comparator]   VARCHAR (100) NULL,
    [CompareValue] VARCHAR (500) NULL,
    CONSTRAINT [PK_FiltersDetails] PRIMARY KEY CLUSTERED ([FDID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[FiltersDetails] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[FiltersDetails] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FiltersDetails] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[FiltersDetails] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FiltersDetails] TO [reader]
    AS [dbo];

