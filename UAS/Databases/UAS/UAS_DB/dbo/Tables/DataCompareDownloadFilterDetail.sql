CREATE TABLE [dbo].[DataCompareDownloadFilterDetail]
(
	DcFilterDetailId int IDENTITY(1,1) NOT NULL,
	DcFilterGroupId int NOT NULL,
	FilterType int NULL,
	[Group] varchar(100) NULL,
	Name varchar(50) NULL,
	[Values] varchar(max) NULL,
	SearchCondition varchar(50) NULL,
	CONSTRAINT [PK_DataCompareDownloadFilterDetail] PRIMARY KEY CLUSTERED ([DcFilterDetailId] ASC) WITH (FILLFACTOR = 90),
	CONSTRAINT [FK_DataCompareDownloadFilterDetail_DataCompareDownloadFilterGroup] FOREIGN KEY([DcFilterGroupId]) REFERENCES [dbo].[DataCompareDownloadFilterGroup] ([DcFilterGroupId])
)
