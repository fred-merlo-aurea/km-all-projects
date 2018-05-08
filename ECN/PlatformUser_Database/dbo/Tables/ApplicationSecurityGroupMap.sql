CREATE TABLE [dbo].[ApplicationSecurityGroupMap] (
    [ApplicationSecurityGroupMapID] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]               INT      NOT NULL,
    [ApplicationID]                 INT      NOT NULL,
    [HasAccess]                     BIT      NOT NULL,
    [DateCreated]                   DATETIME NOT NULL,
    [DateUpdated]                   DATETIME NULL,
    [CreatedByUserID]               INT      NOT NULL,
    [UpdatedByUserID]               INT      NULL,
    CONSTRAINT [PK_ApplicationMap] PRIMARY KEY CLUSTERED ([SecurityGroupID] ASC, [ApplicationID] ASC),
    CONSTRAINT [FK_ApplicationSecurityGroupMap_Application] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Application] ([ApplicationID]),
    CONSTRAINT [FK_ApplicationSecurityGroupMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID])
);



