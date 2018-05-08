CREATE TABLE [dbo].[DomainFieldMasterGroup] (
    [DomainFieldMasterGroupID] INT          IDENTITY (1, 1) NOT NULL,
    [DomainTrackingId]         INT          NOT NULL,
    [DomainName]               VARCHAR (50) NULL,
    [FieldName]                VARCHAR (50) NULL,
    [MasterGroupId]            INT          NOT NULL,
    CONSTRAINT [PK_DomainFieldMasterGroup] PRIMARY KEY CLUSTERED ([DomainFieldMasterGroupID] ASC),
    CONSTRAINT [FK_DomainFieldMasterGroup_DomainTrackingId] FOREIGN KEY ([DomainTrackingId]) REFERENCES [dbo].[DomainTracking] ([DomainTrackingID]),
    CONSTRAINT [FK_DomainFieldMasterGroup_MasterGroupId] FOREIGN KEY ([MasterGroupId]) REFERENCES [dbo].[MasterGroups] ([MasterGroupID])
);

