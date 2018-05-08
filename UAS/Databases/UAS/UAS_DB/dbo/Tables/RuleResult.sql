create table RuleResult
(
	RuleResultId int identity(1,1),
	RuleId int not null,
	RuleFieldId int not null,
	UpdateField	varchar(50) not null,
	UpdateFieldPrefix varchar(5) not null,
	UpdateFieldValue varchar(250) not null,
	IsClientField bit not null,
	IsActive bit default('true') not null,
	CreatedDate	datetime default(getdate()) not null,
	CreatedByUserId int not null,
	UpdatedByUserId int not null, 
    CONSTRAINT [PK_RuleResult] PRIMARY KEY ([RuleResultId])
)
go
