CREATE TABLE [dbo].[MenuSecurityGroupMap] (
    [MenuSecurityGroupMapID] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]        INT      NOT NULL,
    [MenuID]                 INT      NOT NULL,
    [AccessID]               INT      DEFAULT ((0)) NOT NULL,
    [HasAccess]              BIT      CONSTRAINT [DF_MenuMap_HasAccess] DEFAULT ((0)) NOT NULL,
    [IsActive]               BIT      CONSTRAINT [DF_MenuMap_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]            DATETIME NOT NULL,
    [DateUpdated]            DATETIME NULL,
    [CreatedByUserID]        INT      NOT NULL,
    [UpdatedByUserID]        INT      NULL,
    CONSTRAINT [PK_MenuSecurityGroupMap] PRIMARY KEY CLUSTERED ([SecurityGroupID] ASC, [MenuID] ASC, [AccessID] ASC),
    CONSTRAINT [FK_MenuSecurityGroupMap_Menu] FOREIGN KEY ([MenuID]) REFERENCES [dbo].[Menu] ([MenuID]),
    CONSTRAINT [FK_MenuSecurityGroupMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID])
);





