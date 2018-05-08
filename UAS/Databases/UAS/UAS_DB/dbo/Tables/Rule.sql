CREATE TABLE [dbo].[Rule]
(
	RuleId int not null identity(1,1),
	RuleName varchar(250) not null,
	DisplayName varchar(250) not null, 
	RuleDescription varchar(1250) null,
	IsDeleteRule bit default((0)) not null,
	IsSystem bit default((0)) not null,
	IsGlobal bit default ((0)) not null,
	ClientId int not null,
	IsActive bit default((1)) not null,
	DateCreated datetime default(getdate()) not null,
	DateUpdated datetime null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
	CustomImportRuleId int DEFAULT ((0)) not null,
    RuleActionId int DEFAULT ((0)) not null,
    CONSTRAINT [PK_Rule] PRIMARY KEY ([RuleId]) WITH (FILLFACTOR = 90),

)
