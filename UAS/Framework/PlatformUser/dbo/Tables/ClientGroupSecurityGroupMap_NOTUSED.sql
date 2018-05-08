CREATE TABLE [dbo].[ClientGroupSecurityGroupMap_NOTUSED] (
    [ClientGroupSecurityGroupMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ClientGroupID]                 INT      NOT NULL,
    [SecurityGroupID]               INT      NOT NULL,
    [IsActive]                      BIT      NOT NULL,
    [DateCreated]                   DATETIME NOT NULL,
    [DateUpdated]                   DATETIME NULL,
    [CreatedByUserID]               INT      NOT NULL,
    [UpdatedByUserID]               INT      NULL,
    CONSTRAINT [PK_ClientSecurityGroupMap] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC, [SecurityGroupID] ASC),
    CONSTRAINT [FK_ClientGroupSecurityGroupMap_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID])
);

