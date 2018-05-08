create table DataCompareDownloadCostDetail
(
	DcDownloadId int not null,
	CodeTypeId int not null,
	CostPerItemClient decimal(19,4) default((0)) null,
	CostPerItemDetailClient varchar(500)  null,
	CostPerItemThirdParty decimal(19,4) default((0)) null,
	CostPerItemDetailThirdParty varchar(500) null,
	ItemCount int default((0)) null,
	ItemTotalCostClient decimal(19,4) default((0)) null,
	ItemTotalCostThirdParty decimal(19,4) default((0)) null, 
    CONSTRAINT [PK_DataCompareDownloadCostDetail] PRIMARY KEY ([DcDownloadId], [CodeTypeId])
)
