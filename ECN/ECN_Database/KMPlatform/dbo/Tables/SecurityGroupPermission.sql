CREATE TABLE [dbo].[SecurityGroupPermission] (
    [SecurityGroupPermissionID] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]           INT      NOT NULL,
    [ServiceFeatureAccessMapID] INT      NOT NULL,
    [IsActive]                  BIT      NOT NULL,
    [DateCreated]               DATETIME NULL,
    [CreatedByUserID]           INT      NULL,
    [DateUpdated]               DATETIME NULL,
    [UpdatedByUserID]           INT      NULL,
    CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED ([SecurityGroupPermissionID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_SecurityGroupPermission_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID]),
    CONSTRAINT [FK_SecurityGroupPermission_ServiceFeatureAccessMap] FOREIGN KEY ([ServiceFeatureAccessMapID]) REFERENCES [dbo].[ServiceFeatureAccessMap] ([ServiceFeatureAccessMapID])
);
GO

CREATE NONCLUSTERED INDEX [ix_SecurityGroupPermission_SecurityGroupID_IsActive_includes]
    ON [dbo].[SecurityGroupPermission]([SecurityGroupID] ASC, [IsActive] ASC)
    INCLUDE([SecurityGroupPermissionID], [ServiceFeatureAccessMapID], [DateCreated], [CreatedByUserID], [DateUpdated], [UpdatedByUserID]) WITH (FILLFACTOR = 80);
GO
