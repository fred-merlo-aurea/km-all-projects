CREATE TABLE [dbo].[SecurityGroup] (
    [SecurityGroupID]     INT           IDENTITY (1, 1) NOT NULL,
    [SecurityGroupName]   VARCHAR (50)  NOT NULL,
    [ClientGroupID]       INT           NULL,
    [ClientID]            INT           NULL,
    [IsActive]            BIT           CONSTRAINT [DF_SecurityGroup_IsActive] DEFAULT ((0)) NOT NULL,
    [AdministrativeLevel] VARCHAR (50)  NULL,
    [DateCreated]         DATETIME      NOT NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NOT NULL,
    [UpdatedByUserID]     INT           NULL,
    [Description]         VARCHAR (500) NULL,
    CONSTRAINT [PK_SecurityGroup] PRIMARY KEY CLUSTERED ([SecurityGroupID] ASC),
    CONSTRAINT [FK_SecurityGroup_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK_SecurityGroup_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID]),
    CONSTRAINT [IX_SecurityGroup_UNIQUE_KEYS] UNIQUE NONCLUSTERED ([SecurityGroupName] ASC, [ClientID] ASC, [ClientGroupID] ASC)
);





