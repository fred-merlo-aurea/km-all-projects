CREATE TABLE [dbo].[FilterSegmentationGroup]
(
	[FilterSegmentationGroupID] int IDENTITY(1,1)		NOT NULL,
	[FilterSegmentationID]		int						NOT NULL,
	[FilterGroupID_Selected]			varchar (500)			NULL,
	[FilterGroupID_Suppressed]		varchar (500)			NULL,
	[SelectedOperation]			varchar					(50) NULL,
	[SuppressedOperation]		varchar					(50) NULL,
	CONSTRAINT [PK_FilterSegmentationGroup] PRIMARY KEY CLUSTERED ([FilterSegmentationGroupID] ASC) WITH (FILLFACTOR = 80),
	CONSTRAINT [FK_FilterSegmentationGroup_FilterSegmentation] FOREIGN KEY ([FilterSegmentationID]) REFERENCES [dbo].[FilterSegmentation] ([FilterSegmentationID])
)
