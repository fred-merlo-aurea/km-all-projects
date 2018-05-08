CREATE TABLE [dbo].[ClientGroupUserMap] (
    [ClientGroupUserMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ClientGroupID]        INT      NOT NULL,
    [UserID]               INT      NOT NULL,
    [IsActive]             BIT      NOT NULL,
    [DateCreated]          DATETIME NOT NULL,
    [DateUpdated]          DATETIME NULL,
    [CreatedByUserID]      INT      NOT NULL,
    [UpdatedByUserID]      INT      NULL,
    CONSTRAINT [PK_ClientGroupUserMap] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC, [UserID] ASC),
    CONSTRAINT [FK_ClientGroupUserMap_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID]),
    CONSTRAINT [FK_ClientGroupUserMap_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


