create table DataCompareDownload
(
	DcDownloadId int identity(1,1),
	DcViewId int not null,
	WhereClause varchar(max) null,
	DcTypeCodeId int not null,
	ProfileCount int default((0)) null,
	TotalItemCount int default((0)) null,
	TotalBilledCost decimal(19,4) default((0)) null,
	TotalThirdPartyCost decimal(19,4) default((0)) null,
	IsPurchased bit default('false') not null,
	PurchasedByUserId int null,
	PurchasedDate datetime  null,
	PurchasedCaptcha varchar(50) null,
	IsBilled bit default('false') not null,
	BilledDate datetime null,
	DownloadFileName varchar(100) null, 
	CreatedByUserId int null,
    DateCreated datetime null
    CONSTRAINT [PK_DataCompareDownload_DcDownloadId] PRIMARY KEY NONCLUSTERED ([DcDownloadId] ASC),
    CONSTRAINT [FK_DataCompareDownload_DataCompareView] FOREIGN KEY ([DcViewId]) REFERENCES [dbo].[DataCompareView] ([DcViewId])
)