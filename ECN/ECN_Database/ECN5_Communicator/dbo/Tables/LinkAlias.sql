CREATE TABLE [dbo].[LinkAlias] (
    [AliasID]       INT           IDENTITY (1, 1) NOT NULL,
    [ContentID]     INT           NULL,
    [Link]          VARCHAR (2048) NULL,
    [Alias]         VARCHAR (2048) NULL,
    [LinkOwnerID]   INT           NULL,
    [LinkTypeID]    INT           NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_LinkAlias_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [CreatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_LinkAlias_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_LinkAlias] PRIMARY KEY CLUSTERED ([AliasID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_LinkAlias_ContentID]
    ON [dbo].[LinkAlias]([ContentID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_LinkAlias_LinkOwnerID]
    ON [dbo].[LinkAlias]([LinkOwnerID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_LinkAlias_OwnerID_TypeID]
    ON [dbo].[LinkAlias]([LinkOwnerID] ASC, [LinkTypeID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[LinkAlias] TO [reader]
    AS [dbo];

