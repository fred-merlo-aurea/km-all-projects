CREATE TABLE [dbo].[SecurityGroupPermission] (
    [SecurityGroupPermissionID] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]           INT      NOT NULL,
    [ServiceFeatureAccessMapID] INT      NOT NULL,
    [IsActive]                  BIT      NOT NULL,
    [DateCreated]               DATETIME NULL,
    [CreatedByUserID]           INT      NULL,
    [DateUpdated]               DATETIME NULL,
    [UpdatedByUserID]           INT      NULL,
    CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED ([SecurityGroupPermissionID] ASC),
    CONSTRAINT [FK_SecurityGroupPermission_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID]),
    CONSTRAINT [FK_SecurityGroupPermission_ServiceFeatureAccessMap] FOREIGN KEY ([ServiceFeatureAccessMapID]) REFERENCES [dbo].[ServiceFeatureAccessMap] ([ServiceFeatureAccessMapID])
);




GO


