CREATE TABLE [dbo].[Summary_BrandDimension_Data] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [BrandID]       INT           NOT NULL,
    [MasterGroupID] INT           NOT NULL,
    [MasterID]      INT           NULL,
    [MasterValue]   VARCHAR (100) NULL,
    [MasterDesc]    VARCHAR (255) NULL,
    [Counts]        BIGINT        CONSTRAINT [DF__Summary_BrandDimension_Data_A__recor__2DA7A64D] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Summary_BrandDimension_Data] PRIMARY KEY CLUSTERED ([ID] ASC)
);

