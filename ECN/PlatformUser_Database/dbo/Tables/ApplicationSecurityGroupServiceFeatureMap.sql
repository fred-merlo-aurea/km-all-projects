CREATE TABLE [dbo].[ApplicationSecurityGroupServiceFeatureMap] (
    [ApplicationSecurityGroupServiceFeatureMap] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]                           INT      NOT NULL,
    [ApplicationID]                             INT      NOT NULL,
    [ServiceFeatureID]                          INT      NOT NULL,
    [ServiceID]                                 INT      NOT NULL,
    [HasAccess]                                 BIT      NOT NULL,
    [DateCreated]                               DATETIME NOT NULL,
    [DateUpdated]                               DATETIME NULL,
    [CreatedByUserID]                           INT      NOT NULL,
    [UpdatedByUserID]                           INT      NULL,
    CONSTRAINT [PK_ApplicationSecurityGroupServiceFeatureMap] PRIMARY KEY CLUSTERED ([SecurityGroupID] ASC, [ApplicationID] ASC, [ServiceFeatureID] ASC),
    CONSTRAINT [FK_ApplicationSecurityGroupServiceFeatureMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID]),
    CONSTRAINT [FK_ApplicationSecurityGroupServiceFeatureMap_Service] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Service] ([ServiceID]),
    CONSTRAINT [FK_ApplicationSecurityGroupServiceFeatureMap_ServiceFeature] FOREIGN KEY ([ServiceFeatureID]) REFERENCES [dbo].[ServiceFeature] ([ServiceFeatureID])
);

