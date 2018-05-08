CREATE TABLE [dbo].[FilterGroup] (
    [FilterGroupID]        INT          IDENTITY (1, 1) NOT NULL,
    [FilterID]             INT          NOT NULL,
    [SortOrder]            INT          NOT NULL,
    [Name]                 VARCHAR (50) NULL,
    [ConditionCompareType] VARCHAR (50) NULL,
    [CreatedDate]          DATETIME     CONSTRAINT [DF_FilterGroup_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]        INT          NULL,
    [IsDeleted]            BIT          CONSTRAINT [DF_FilterGroup_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]          DATETIME     NULL,
    [UpdatedUserID]        INT          NULL,
    CONSTRAINT [PK_FilterGroup] PRIMARY KEY CLUSTERED ([FilterGroupID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_FilterGroup_Filter] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filter] ([FilterID])
);

