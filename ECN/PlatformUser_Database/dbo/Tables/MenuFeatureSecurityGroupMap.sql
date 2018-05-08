CREATE TABLE [dbo].[MenuFeatureSecurityGroupMap] (
    [MenuFeatureSecurityGroupMapID] INT      IDENTITY (1, 1) NOT NULL,
    [MenuFeatureID]                 INT      NOT NULL,
    [SecurityGroupID]               INT      NOT NULL,
    [AccessID]                      INT      NOT NULL,
    [HasAccess]                     BIT      CONSTRAINT [DF_MenuFeatureSecurityGroupMap_HasAccess] DEFAULT ((0)) NOT NULL,
    [IsActive]                      BIT      CONSTRAINT [DF_MenuFeatureSecurityGroupMap_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]                   DATETIME NOT NULL,
    [DateUpdated]                   DATETIME NULL,
    [CreatedByUserID]               INT      NOT NULL,
    [UpdatedByUserID]               INT      NULL,
    CONSTRAINT [PK_MenuFeatureSecurityGroupMap] PRIMARY KEY CLUSTERED ([MenuFeatureID] ASC, [SecurityGroupID] ASC, [AccessID] ASC),
    CONSTRAINT [FK_MenuFeatureSecurityGroupMap_MenuFeature] FOREIGN KEY ([MenuFeatureID]) REFERENCES [dbo].[MenuFeature] ([MenuFeatureID]),
    CONSTRAINT [FK_MenuFeatureSecurityGroupMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID])
);




