CREATE TABLE [dbo].[MenuServiceFeatureMap](
      [MenuSFMapID] [int] IDENTITY(1,1) NOT NULL,
      [MenuID] [int] NOT NULL,
      [ServiceFeatureID] [int] NOT NULL,
      [IsActive] [bit] NOT NULL,
      [DateCreated] [datetime] NOT NULL,
      [DateUpdated] [datetime] NULL,
      [CreatedByUserID] [int] NOT NULL,
      [UpdatedByUserID] [int] NULL,
CONSTRAINT [PK_MenuServiceFeatureMap] PRIMARY KEY CLUSTERED 
(
      [MenuSFMapID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MenuServiceFeatureMap]  WITH CHECK ADD  CONSTRAINT [FK_MenuServiceFeatureMap_Menu] FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menu] ([MenuID])
GO

ALTER TABLE [dbo].[MenuServiceFeatureMap] CHECK CONSTRAINT [FK_MenuServiceFeatureMap_Menu]
GO

ALTER TABLE [dbo].[MenuServiceFeatureMap]  WITH CHECK ADD  CONSTRAINT [FK_MenuServiceFeatureMap_ServiceFeature] FOREIGN KEY([ServiceFeatureID])
REFERENCES [dbo].[ServiceFeature] ([ServiceFeatureID])
GO

ALTER TABLE [dbo].[MenuServiceFeatureMap] CHECK CONSTRAINT [FK_MenuServiceFeatureMap_ServiceFeature]
GO

ALTER TABLE [dbo].[MenuServiceFeatureMap] ADD  CONSTRAINT [DF_MenuServiceFeatureMap_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[MenuServiceFeatureMap] ADD  CONSTRAINT [DF_MenuServiceFeatureMap_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

