create table RuleField
(
	RuleFieldId	int identity(1,1),
	ClientId int default((0)) not null,--for Subscriptions, PubSubscriptions and SubscriberFinal will use a ClientId of 0 / IsClientField = false
	IsClientField bit default('false') not null,
	DataTable varchar(75) not null,
	TablePrefix	varchar(5) not null,
	Field varchar(50) not null,
	DataType varchar(50) not null,--just going to create Enum
	UIControl varchar(50) not null,--just going to create Enum 
	IsMultiSelect bit default('false') not null,
	IsActive bit default('true') not null,
	CreatedDate	datetime default(getdate()) not null,
	CreatedByUserId int not null,
	UpdatedByUserId int not null, 
    CONSTRAINT [PK_RuleField] PRIMARY KEY ([ClientId], [DataTable], [Field]) 
)
go
