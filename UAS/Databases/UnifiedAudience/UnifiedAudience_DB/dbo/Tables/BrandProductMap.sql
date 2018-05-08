CREATE TABLE [dbo].[BrandProductMap] (
    [BrandProductMapID] INT      IDENTITY (1, 1) NOT NULL,
    [BrandID]           INT      NOT NULL,
    [ProductID]         INT      NOT NULL,
    [HasAccess]         BIT      DEFAULT ('false') NOT NULL,
    [DateCreated]       DATETIME CONSTRAINT [DF_BrandProductMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]       DATETIME NULL,
    [CreatedByUserID]   INT      NOT NULL,
    [UpdatedByUserID]   INT      NULL,
    CONSTRAINT [PK_BrandProductMap] PRIMARY KEY CLUSTERED ([BrandID] ASC, [ProductID] ASC) WITH (FILLFACTOR = 90)
);

