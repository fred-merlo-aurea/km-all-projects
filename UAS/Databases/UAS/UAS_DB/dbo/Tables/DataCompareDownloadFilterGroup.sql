CREATE TABLE [dbo].[DataCompareDownloadFilterGroup]
(
	DcFilterGroupId int IDENTITY(1,1) NOT NULL,
	DcDownloadId int NOT NULL,
	CONSTRAINT [PK_DataCompareDownloadFilterGroup] PRIMARY KEY CLUSTERED ([DcFilterGroupId] ASC) WITH (FILLFACTOR = 90),
	CONSTRAINT [FK_DataCompareDownloadFilterGroup_DataCompareDownload] FOREIGN KEY([DcDownloadId]) REFERENCES [dbo].[DataCompareDownload] ([DcDownloadId])
)