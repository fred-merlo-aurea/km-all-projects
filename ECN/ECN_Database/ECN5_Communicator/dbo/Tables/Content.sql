CREATE TABLE [dbo].[Content] (
    [ContentID]          INT           IDENTITY (1, 1) NOT NULL,
    [CreatedUserID]      INT           NULL,
    [FolderID]           INT           CONSTRAINT [DF_Content_FolderID] DEFAULT (0) NULL,
    [LockedFlag]         VARCHAR (1)   NULL,
    [ContentSource]      TEXT          NULL,
    [ContentText]        TEXT          NULL,
    [ContentTypeCode]    VARCHAR (50)  NULL,
    [ContentCode]        VARCHAR (50)  NULL,
    [ContentTitle]       VARCHAR (255) NULL,
    [CustomerID]         INT           NULL,
    [ContentURL]         VARCHAR (255) NULL,
    [ContentFilePointer] VARCHAR (255) NULL,
    [UpdatedDate]        DATETIME      CONSTRAINT [DF_Content_UpdatedDate] DEFAULT (getdate()) NULL,
    [Sharing]            CHAR (1)      CONSTRAINT [DF_Content_Sharing] DEFAULT ('N') NULL,
    [MasterContentID]    INT           NULL,
    [ContentMobile]      TEXT          NULL,
    [ContentSMS]         TEXT          NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_Content_CREATEDDATE] DEFAULT (getdate()) NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_Content_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID]      INT           NULL,
    [UseWYSIWYGeditor]   BIT           NULL,
    [Archived] BIT NULL, 
    [IsValidated] BIT NULL DEFAULT ((1)), 
    CONSTRAINT [Content_PK] PRIMARY KEY CLUSTERED ([ContentID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_Content_FolderID_CustomerID]
    ON [dbo].[Content]([FolderID] ASC, [CustomerID] ASC)
    INCLUDE([ContentID], [LockedFlag], [ContentTypeCode], [ContentTitle], [UpdatedDate]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_Content_4]
    ON [dbo].[Content]([FolderID] ASC)
    INCLUDE([ContentID]) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Content] TO [reader]
    AS [dbo];

