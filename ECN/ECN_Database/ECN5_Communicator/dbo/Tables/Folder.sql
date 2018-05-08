CREATE TABLE [dbo].[Folder] (
    [FolderID]          INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]        INT           NULL,
    [FolderName]        VARCHAR (50)  NULL,
    [FolderDescription] VARCHAR (255) NULL,
    [FolderType]        VARCHAR (50)  CONSTRAINT [DF_Folders_FolderType] DEFAULT ('CNT') NOT NULL,
    [IsSystem]          BIT           NULL,
    [ParentID]          INT           CONSTRAINT [DF_Folders_ParentID] DEFAULT (0) NOT NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_Folder_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]     INT           NULL,
    [IsDeleted]         BIT           CONSTRAINT [DF_Folder_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]       DATETIME      NULL,
    [UpdatedUserID]     INT           NULL,
    CONSTRAINT [PK_Folders] PRIMARY KEY CLUSTERED ([FolderID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [idx_Folder_CustomerID_FolderType_isDeleted]
    ON [dbo].[Folder]([CustomerID] ASC, [FolderType] ASC, [IsDeleted] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Folder] TO [reader]
    AS [dbo];

