CREATE TABLE SubGenQue
(
	SubGenQueId INT NOT NULL Identity(1,1) PRIMARY KEY,
	QueTypeCodeId int not null,
	SubGenEntityCodeId int not null,
	ClientId int not null,
	SubGenAccountId int not null,
	ProductId int not null,
	SubGenPublicationId int not null,
	JsonData varchar(max) not null,
	IsProcessed bit not null,
	ProcessedDate date null,
	ProcessedTime time null,
	ProcessCode varchar(50) null,
	DateCreated   datetime not null,
	DateUpdated   datetime null
)
