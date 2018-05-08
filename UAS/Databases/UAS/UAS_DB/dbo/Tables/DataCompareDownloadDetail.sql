CREATE TABLE [dbo].[DataCompareDownloadDetail]
(
	DcDownloadDetailId int IDENTITY(1,1) NOT NULL,
	DcDownloadId int NOT NULL,
	SubscriptionId int NOT NULL,
	CONSTRAINT [PK_DataCompareDownloadDetail] PRIMARY KEY CLUSTERED ([DcDownloadDetailId] ASC) WITH (FILLFACTOR = 90),
	CONSTRAINT [FK_DataCompareDownloadDetail_DataCompareDownload] FOREIGN KEY([DcDownloadId]) REFERENCES [dbo].[DataCompareDownload] ([DcDownloadId])
)
