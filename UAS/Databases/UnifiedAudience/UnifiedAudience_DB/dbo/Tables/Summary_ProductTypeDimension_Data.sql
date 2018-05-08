CREATE TABLE [dbo].[Summary_ProductTypeDimension_Data] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [PubTypeID]     INT           NOT NULL,
    [MasterGroupID] INT           NOT NULL,
    [MasterID]      INT           NULL,
    [MasterValue]   VARCHAR (100) NULL,
    [MasterDesc]    VARCHAR (255) NULL,
    [Counts]        BIGINT        CONSTRAINT [DF__Summary_ProductTypeDimension_Data_A__recor__2DA7A64D] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Summary_ProductTypeDimension_Data] PRIMARY KEY CLUSTERED ([ID] ASC)
);

