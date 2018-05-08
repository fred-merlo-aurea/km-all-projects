CREATE TABLE [dbo].[DataCompareDownloadField]
(
	DcDownloadFieldId int IDENTITY(1,1) NOT NULL,
	DcDownloadId int NOT NULL,
	DcDownloadFieldCodeId int NULL,
	ColumnName varchar(50) NULL,
	ColumnID int NULL,
	IsDescription bit NULL,
    CONSTRAINT [PK_DataCompareDownloadField] PRIMARY KEY CLUSTERED ([DcDownloadFieldId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DataCompareDownloadField_DataCompareDownload] FOREIGN KEY ([DcDownloadId]) REFERENCES [dbo].[DataCompareDownload] ([DcDownloadId])
)
