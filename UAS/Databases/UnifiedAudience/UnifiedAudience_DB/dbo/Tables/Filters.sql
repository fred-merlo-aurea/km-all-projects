CREATE TABLE [dbo].[Filters] (
    [FilterID]           INT                          IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (50)                 NOT NULL,
    [FilterXML]          XML(CONTENT [dbo].[Filters]) NULL,
    [CreatedDate]        DATETIME                     NULL,
    [CreatedUserID]      INT                          NULL,
    [FilterType]         VARCHAR (50)                 NULL,
    [PubID]              INT                          NULL,
    [IsDeleted]          BIT                          CONSTRAINT [DF_Filters_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]        DATETIME                     NULL,
    [UpdatedUserID]      INT                          NULL,
    [BrandID]            INT                          NULL,
    [AddtoSalesView]     BIT                          NULL,
    [FilterCategoryID]   INT                          NULL,
    [QuestionCategoryID] INT                          NULL,
    [QuestionName]       VARCHAR (200)                NULL,
    [IsLocked]           BIT                          CONSTRAINT [DF_Filters_IsLocked] DEFAULT ((0)) NULL,
    [Notes] VARCHAR(250) NULL, 
    CONSTRAINT [PK_Filters] PRIMARY KEY CLUSTERED ([FilterID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Filters_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);
GO

CREATE NONCLUSTERED INDEX [IDX_Filters_FilterType]
    ON [dbo].[Filters]([FilterType] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Filters_Name]
    ON [dbo].[Filters]([Name] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Filters_PubID]
    ON [dbo].[Filters]([PubID] ASC) WITH (FILLFACTOR = 90);
GO

