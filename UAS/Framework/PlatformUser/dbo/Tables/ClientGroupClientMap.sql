CREATE TABLE [dbo].[ClientGroupClientMap] (
    [ClientGroupClientMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ClientGroupID]          INT      NOT NULL,
    [ClientID]               INT      NOT NULL,
    [IsActive]               BIT      NOT NULL,
    [DateCreated]            DATETIME NOT NULL,
    [DateUpdated]            DATETIME NULL,
    [CreatedByUserID]        INT      NOT NULL,
    [UpdatedByUserID]        INT      NULL,
    CONSTRAINT [PK_ClientGroupClientMap] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC, [ClientID] ASC),
    CONSTRAINT [FK_ClientGroupClientMap_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK_ClientGroupClientMap_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID])
);

