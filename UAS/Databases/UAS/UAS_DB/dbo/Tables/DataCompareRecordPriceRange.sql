create table DataCompareRecordPriceRange
(
	DcRecordPriceRangeId int identity(1,1),
	MinCount bigint not null,
	MaxCount bigint not null,
	MatchMergePurgeCost decimal(20, 10) NOT NULL,
	MatchPricePerRecord decimal(20,10) not null,
	LikeMergePurgeCost decimal(20, 10) NOT NULL,
	LikePricePerRecord decimal(20,10) not null,
	IsMergePurgePerRecordPricing bit default('false') not null,
	IsActive bit default('true') not null,
	DateCreated datetime not null,
	CreatedByUserId int not null,
	DateUpdated datetime null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_DataCompareRecordPriceRange] PRIMARY KEY ([DcRecordPriceRangeId])
)

