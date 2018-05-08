CREATE TABLE [dbo].[ContentFilterDetail] (
    [FDID]          INT           IDENTITY (100, 1) NOT NULL,
    [FilterID]      INT           NULL,
    [FieldType]     VARCHAR (50)  NULL,
    [CompareType]   VARCHAR (50)  NULL,
    [FieldName]     VARCHAR (100) NULL,
    [Comparator]    VARCHAR (100) NULL,
    [CompareValue]  VARCHAR (500) NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_ContentFilterDetail_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_ContentFilterDetail_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_ContentFiltersDetails] PRIMARY KEY CLUSTERED ([FDID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ContentFilterDetail] TO [reader]
    AS [dbo];

