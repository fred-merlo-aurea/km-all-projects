create table DataCompareCostUser
(
	ClientId int not null,
	UserId int not null,
	CodeTypeId int NOT NULL,
	CodeTypeCostModifier decimal(20,10) default(0) not null,
	DateCreated datetime default(GETDATE()) not null,
	CreatedByUserId int default(0) not null,
	DateUpdated datetime null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_DataCompareCostUser] PRIMARY KEY ([ClientId], [CodeTypeId], [UserId])
)