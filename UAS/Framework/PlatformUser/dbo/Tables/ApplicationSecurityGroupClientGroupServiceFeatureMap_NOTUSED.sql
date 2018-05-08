CREATE TABLE [dbo].[ApplicationSecurityGroupClientGroupServiceFeatureMap_NOTUSED] (
    [ApplicationSecurityGroupClientGroupServiceFeatureMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ApplicationID]                                          INT      NOT NULL,
    [SecurityGroupID]                                        INT      NOT NULL,
    [ClientGroupID]                                          INT      NOT NULL,
    [ServiceFeatureID]                                       INT      NOT NULL,
    [ServiceID]                                              INT      NOT NULL,
    [HasAccess]                                              BIT      NOT NULL,
    [DateCreated]                                            DATETIME NOT NULL,
    [DateUpdated]                                            DATETIME NULL,
    [CreatedByUserID]                                        INT      NOT NULL,
    [UpdatedByUserID]                                        INT      NULL,
    CONSTRAINT [PK_ApplicationSecurityGroupClientGroupServiceServiceFeatureMap] PRIMARY KEY CLUSTERED ([ApplicationID] ASC, [SecurityGroupID] ASC, [ClientGroupID] ASC, [ServiceID] ASC, [ServiceFeatureID] ASC),
    CONSTRAINT [FK_ApplicationSecurityGroupClientGroupServiceFeatureMap_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID]),
    CONSTRAINT [FK_ApplicationSecurityGroupClientGroupServiceFeatureMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID]),
    CONSTRAINT [FK_ApplicationSecurityGroupClientGroupServiceFeatureMap_Service] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Service] ([ServiceID]),
    CONSTRAINT [FK_ApplicationSecurityGroupClientGroupServiceFeatureMap_ServiceFeature] FOREIGN KEY ([ServiceFeatureID]) REFERENCES [dbo].[ServiceFeature] ([ServiceFeatureID])
);

