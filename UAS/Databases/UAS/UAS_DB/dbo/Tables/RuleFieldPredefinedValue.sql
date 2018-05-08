create table RuleFieldPredefinedValue
(
	RuleFieldPredefinedValueId	int identity(1,1),
	RuleFieldId	int not null,
	ItemText	varchar(250) not null,
	ItemValue	varchar(250) not null,
	ItemOrder	int not null,
	IsActive bit default('true') not null,
	CreatedDate	datetime default(getdate()) not null,
	CreatedByUserId int not null,
	UpdatedByUserId int not null, 
    CONSTRAINT [PK_RuleFieldPredefinedValue] PRIMARY KEY ([RuleFieldPredefinedValueId])
)
go
