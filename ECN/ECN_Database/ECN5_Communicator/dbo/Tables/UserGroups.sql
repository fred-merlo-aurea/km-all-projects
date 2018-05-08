CREATE TABLE [dbo].[UserGroups] (
    [UGID]          INT      IDENTITY (1, 1) NOT NULL,
    [UserID]        INT      NOT NULL,
    [GroupID]       INT      NOT NULL,
    [CreatedDate]   DATETIME CONSTRAINT [DF_UserGroups_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT      NULL,
    [IsDeleted]     BIT      CONSTRAINT [DF_UserGroups_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME NULL,
    [UpdatedUserID] INT      NULL,
    CONSTRAINT [PK_UserGroups] PRIMARY KEY CLUSTERED ([UGID] ASC) WITH (FILLFACTOR = 80)
);

