CREATE TABLE [dbo].[Summary_PubTypeBrandDimension_Data] (
   [ID] [int] IDENTITY (1, 1) NOT NULL,
   [PubTypeID] [int] NOT NULL,
   [BrandID] [int] NOT NULL,
   [MasterGroupID] [int] NOT NULL,
   [MasterID] [int] NULL,
   [MasterValue] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
   [MasterDesc] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
   [Counts] [bigint] NOT NULL CONSTRAINT [DF__Summary_PubTypeBrandDimension_Data_counts] DEFAULT ((0)),
    CONSTRAINT [PK_Summary_PubTypeBrandDimension_Data] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 100)
)
GO
CREATE INDEX [IX_Summary_PubTypeBrandDimension_BrandID_PubTypeID_MastergroupID] ON [dbo].[Summary_PubTypeBrandDimension_Data] ([BrandID], [PubTypeID], [MasterGroupID])