create table RuleCondition
(
	RuleId int not null,
	Line int not null,
	IsGrouped bit not null,
	GroupNumber	int null,
	ChainId	int not null,
	CompareField varchar(50) not null,
	CompareFieldPrefix varchar(5),
	IsClientField bit not null,
	OperatorId int not null,
	CompareValue varchar(250) not null,
	IsActive bit default('true') not null,
	CreatedDate datetime default(getdate()) not null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_RuleCondition] PRIMARY KEY ([RuleId], [Line]) 
)
go
