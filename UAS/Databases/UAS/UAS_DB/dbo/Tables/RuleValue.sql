CREATE TABLE [dbo].[RuleValue]
(
	RuleValueId int not null identity(1,1),
	ClientId int default(0) not null,
	DisplayValue varchar(250) not null,
	[Value] varchar(250) not null, 
	ValueDescription varchar(1250) null,
	RuleValueTypeId int not null,
	IsDefault bit default((0)) not null,
	IsActive bit default((1)) not null,
	IsSystem bit default((0)) not null,
	DateCreated datetime default(getdate()) not null,
	DateUpdated datetime null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_RuleValue] PRIMARY KEY ([RuleValueId]) WITH (FILLFACTOR = 90)
)
