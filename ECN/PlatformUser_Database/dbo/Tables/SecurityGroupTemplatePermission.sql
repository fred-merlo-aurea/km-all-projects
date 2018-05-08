CREATE TABLE [dbo].[SecurityGroupTemplatePermission] (
    [SecurityGroupTemplatePermissionID] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupTemplateID]           INT      NOT NULL,
    [ServiceFeatureAccessMapID]         INT      NOT NULL,
    [IsActive]                          BIT      NOT NULL,
    [DateCreated]                       DATETIME CONSTRAINT [DF__SecurityG__DateC__2AD55B43] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]                       DATETIME NULL,
    [CreatedByUserID]                   INT      NOT NULL,
    [UpdatedByUserID]                   INT      NULL,
    CONSTRAINT [PK_SecurityGroupTemplatePermission] PRIMARY KEY CLUSTERED ([SecurityGroupTemplatePermissionID] ASC),
    CONSTRAINT [FK_SecurityGroupTemplatePermission_SecurityGroupTemplate] FOREIGN KEY ([SecurityGroupTemplateID]) REFERENCES [dbo].[SecurityGroupTemplate] ([SecurityGroupTemplateID])
);



