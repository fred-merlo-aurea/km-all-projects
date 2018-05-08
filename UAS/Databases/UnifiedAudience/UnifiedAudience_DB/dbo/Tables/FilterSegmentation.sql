CREATE TABLE [dbo].[FilterSegmentation]
(
	[FilterSegmentationID]          INT						IDENTITY (1, 1) NOT NULL,
	[FilterSegmentationName]	    VARCHAR(50)				NOT NULL,
	[Notes]							varchar(250)			NULL,
	[FilterID]						int						NOT NULL,
	[IsDeleted]						bit 					CONSTRAINT [DF_FilterSegmentation_IsDeleted] DEFAULT ((0)) NULL,
	[CreatedUserID]					int						NULL,
	[CreatedDate]					datetime				NULL,
	[UpdatedUserID]					int						NULL,
	[UpdatedDate]					datetime				NULL,
	CONSTRAINT [PK_FilterSegmentation] PRIMARY KEY CLUSTERED ([FilterSegmentationID] ASC) WITH (FILLFACTOR = 80),
	CONSTRAINT [FK_FilterSegmentation_Filters] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filters] ([FilterID])
)
