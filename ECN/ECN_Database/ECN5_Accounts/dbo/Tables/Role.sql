CREATE TABLE [dbo].[Role] (
    [RoleID]        INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT          NULL,
    [RoleName]      VARCHAR (50) NULL,
    [CreatedDate]   DATETIME     CONSTRAINT [DF_Role_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_Role_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleID] ASC) WITH (FILLFACTOR = 80)
);

