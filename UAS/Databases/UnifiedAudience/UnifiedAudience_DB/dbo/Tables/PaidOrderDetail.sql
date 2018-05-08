Create Table PaidOrderDetail
(
	PaidOrderDetailId int identity(1,1),
	PaidOrderId int not null,
	ProductSubscriptionId int not null,
	ProductId int not null,
	RefundDate date null,
	FulfilledDate date null,
	SubTotal money default(0) not null,
	TaxTotal money default(0) not null,
	GrandTotal money default(0) not null,
	SubGenBundleId int null,
	SubGenOrderItemId int null,
	DateCreated datetime not null,
	DateUpdated datetime null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_PaidOrderDetail] PRIMARY KEY ([PaidOrderDetailId])
)
go

ALTER TABLE PaidOrderDetail  WITH CHECK ADD  CONSTRAINT [FK_PaidOrderDetail_PaidOrder] FOREIGN KEY(PaidOrderId)
REFERENCES PaidOrder (PaidOrderId)
go
