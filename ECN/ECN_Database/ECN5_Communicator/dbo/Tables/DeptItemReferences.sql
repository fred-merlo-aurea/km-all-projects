CREATE TABLE [dbo].[DeptItemReferences] (
    [DeptItemRefID] INT          IDENTITY (1, 1) NOT NULL,
    [DepartmentID]  INT          NULL,
    [Item]          VARCHAR (50) CONSTRAINT [DF_DepartmentItemReference_Item] DEFAULT ('') NULL,
    [ItemID]        INT          NULL,
    [Share]         CHAR (10)    CONSTRAINT [DF_DepartmentItemReference_Share] DEFAULT ('N') NULL,
    [CreatedDate]   DATETIME     CONSTRAINT [DF_DepartmentItemReference_DateAssigned] DEFAULT (getdate()) NULL,
    [UpdatedDate]   DATETIME     CONSTRAINT [DF_DepartmentItemReference_DateUpdated] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_DeptItemReferences_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID] INT          NULL,
    CONSTRAINT [PK_DepartmentItemReference] PRIMARY KEY CLUSTERED ([DeptItemRefID] ASC) WITH (FILLFACTOR = 80)
);

