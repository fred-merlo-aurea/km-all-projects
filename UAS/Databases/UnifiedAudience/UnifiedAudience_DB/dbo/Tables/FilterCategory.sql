CREATE TABLE [dbo].[FilterCategory] (
    [FilterCategoryID] INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName]     VARCHAR (50) NULL,
    [CreatedUserID]    INT          NULL,
    [CreatedDate]      DATETIME     NULL,
    [UpdatedUserID]    INT          NULL,
    [UpdatedDate]      DATETIME     NULL,
    [IsDeleted]        BIT          CONSTRAINT [DF_FilterCategory_IsDeleted] DEFAULT ((0)) NULL,
    [ParentID] INT NULL, 
    CONSTRAINT [PK_FilterCategory] PRIMARY KEY CLUSTERED ([FilterCategoryID] ASC)
);

