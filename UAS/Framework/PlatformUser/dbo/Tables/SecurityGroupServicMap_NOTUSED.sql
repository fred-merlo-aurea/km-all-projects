CREATE TABLE [dbo].[SecurityGroupServicMap_NOTUSED] (
    [SecurityGroupServicMap] INT      IDENTITY (1, 1) NOT NULL,
    [SecurityGroupID]        INT      NOT NULL,
    [ServiceID]              INT      NOT NULL,
    [IsEnabled]              BIT      DEFAULT ('false') NOT NULL,
    [DateCreated]            DATETIME NOT NULL,
    [DateUpdatd]             DATETIME NULL,
    [CreatedByUserID]        INT      NOT NULL,
    [UpdatedByUserID]        INT      NULL,
    CONSTRAINT [PK_SecurityGroupServicMap] PRIMARY KEY CLUSTERED ([SecurityGroupID] ASC, [ServiceID] ASC),
    CONSTRAINT [FK_SecurityGroupServicMap_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [dbo].[SecurityGroup] ([SecurityGroupID])
);

