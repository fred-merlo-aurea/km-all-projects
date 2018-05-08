CREATE TABLE [dbo].[SecurityGroupBrandMap] (
    [SecurityGroupBrandMapID] INT      IDENTITY (1, 1) NOT NULL,
    [BrandID]                 INT      NOT NULL,
    [SecurityGroupID]         INT      NOT NULL,
    [HasAccess]               BIT      DEFAULT ('false') NOT NULL,
    [DateCreated]             DATETIME CONSTRAINT [DF_SecurityGroupBrandMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]             DATETIME NULL,
    [CreatedByUserID]         INT      NOT NULL,
    [UpdatedByUserID]         INT      NULL,
    CONSTRAINT [PK_SecurityGroupBrandMap] PRIMARY KEY CLUSTERED ([BrandID] ASC, [SecurityGroupID] ASC)
);


