CREATE TABLE [dbo].[FilterCondition] (
    [FilterConditionID] INT           IDENTITY (1, 1) NOT NULL,
    [FilterGroupID]     INT           NOT NULL,
    [SortOrder]         INT           NOT NULL,
    [Field]             VARCHAR (100) NOT NULL,
    [FieldType]         VARCHAR (50)  NOT NULL,
    [Comparator]        VARCHAR (100) NOT NULL,
    [CompareValue]      VARCHAR (500) NOT NULL,
    [NotComparator]     INT           NULL,
    [DatePart]          VARCHAR (10)  NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_FilterCondition_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]     INT           NULL,
    [IsDeleted]         BIT           CONSTRAINT [DF_FilterCondition_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]       DATETIME      NULL,
    [UpdatedUserID]     INT           NULL,
    CONSTRAINT [PK_FilterConditions] PRIMARY KEY CLUSTERED ([FilterConditionID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_FilterCondition_FilterGroup] FOREIGN KEY ([FilterGroupID]) REFERENCES [dbo].[FilterGroup] ([FilterGroupID])
);

