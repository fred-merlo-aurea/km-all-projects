create table DataCompareCostBase
(
	CodeTypeId int not null,
	CostPerItem decimal(20,10) default(0) not null,
	DateUpdated datetime default(GETDATE()) not null,
	UpdatedByUserId int default(0) not null,
	CONSTRAINT PK_DataCompareCostBase_CodeTypeId PRIMARY KEY NONCLUSTERED (CodeTypeId)
)
