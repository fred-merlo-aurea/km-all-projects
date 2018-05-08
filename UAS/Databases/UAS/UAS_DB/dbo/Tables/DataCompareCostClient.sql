create table DataCompareCostClient
(
	ClientId int not null,
	CodeTypeId int not null,
	CodeTypeCostModifier decimal(20,10) default(0) not null,
	DateCreated datetime default(GETDATE()) not null,
	CreatedByUserId int default(0) not null,
	DateUpdated datetime null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_DataCompareCostClient] PRIMARY KEY ([ClientId], [CodeTypeId])
)