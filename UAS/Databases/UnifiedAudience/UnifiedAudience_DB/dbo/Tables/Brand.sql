CREATE TABLE [dbo].[Brand] (
    [BrandID]       INT           IDENTITY (1, 1) NOT NULL,
    [BrandName]     VARCHAR (50)  NULL,
    [Logo]          VARCHAR (100) NULL,
    [IsBrandGroup]  BIT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Brand_IsDeleted] DEFAULT ((0)) NULL,
    [CreatedUserID] INT           NULL,
    [CreatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED ([BrandID] ASC)
);

