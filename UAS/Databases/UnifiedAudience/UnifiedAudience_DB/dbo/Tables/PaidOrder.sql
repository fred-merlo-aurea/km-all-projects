Create Table PaidOrder
(
	PaidOrderId int identity(1,1),
	SubscriptionId int not null,
	ImportName varchar(30) null,
	OrderDate date null,
	IsGift bit default('false') not null,
	SubTotal money default(0) not null,
	TaxTotal money default(0) not null,
	GrandTotal money default(0) not null,
	PaymentAmount money default(0) not null,
	PaymentNote varchar(50) null,
	PaymentTransactionId varchar(50) null,
	PaymentTypeCodeId int null,
	UserId int null,
	SubGenOrderId int null,
	SubGenSubscriberId int null,
	SubGenUserId int null,
	DateCreated datetime not null,
	DateUpdated datetime null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_PaidOrder] PRIMARY KEY ([PaidOrderId])
)