CREATE TABLE [dbo].[UserClientSecurityGroupMap] (
    [UserClientSecurityGroupMapID] INT          IDENTITY (1, 1) NOT NULL,
    [UserID]                       INT          NOT NULL,
    [ClientID]                     INT          NOT NULL,
    [SecurityGroupID]              INT          NOT NULL,
    [IsActive]                     BIT          NOT NULL,
    [DateCreated]                  DATETIME     NOT NULL,
    [DateUpdated]                  DATETIME     NULL,
    [CreatedByUserID]              INT          NOT NULL,
    [UpdatedByUserID]              INT          NULL,
    [InactiveReason]               VARCHAR (50) NULL,
    CONSTRAINT [PK_UserClientSecurityGroupMap] PRIMARY KEY CLUSTERED ([UserID] ASC, [ClientID] ASC, [SecurityGroupID] ASC),
    CONSTRAINT [FK_UserClientSecurityGroupMap_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK_UserClientSecurityGroupMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID]),
    CONSTRAINT [FK_UserClientSecurityGroupMap_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);

