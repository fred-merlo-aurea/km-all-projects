CREATE TABLE [dbo].[SecurityGroupProductMap] (
    [SecurityGroupProductMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ProductID]                 INT      NOT NULL,
    [SecurityGroupID]           INT      NOT NULL,
    [HasAccess]                 BIT      DEFAULT ('false') NOT NULL,
    [DateCreated]               DATETIME CONSTRAINT [DF_SecurityGroupProductMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]               DATETIME NULL,
    [CreatedByUserID]           INT      NOT NULL,
    [UpdatedByUserID]           INT      NULL,
    CONSTRAINT [PK_SecurityGroupProductMap] PRIMARY KEY CLUSTERED ([ProductID] ASC, [SecurityGroupID] ASC)
);


