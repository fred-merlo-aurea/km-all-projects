CREATE TABLE [dbo].[AcsShippingDetail]
(
	AcsShippingDetailId int identity(1,1) not null Primary Key,
	ClientId int not null,
	CustomerNumber int not null,
	AcsDate date not null,
	ShipmentNumber bigint not null,
	AcsTypeId int not null,
	AcsId int not null,
	AcsName varchar(250) not null,
	ProductCode varchar(100) not null,
	Description varchar(250) not null,
	Quantity int not null,
	UnitCost decimal(8,2) not null,
	TotalCost decimal(12,2) not null,
	DateCreated datetime not null,
	IsBilled bit default('false') not null,
	BilledDate datetime null,
	BilledByUserID int null,
	ProcessCode varchar(50)
)
