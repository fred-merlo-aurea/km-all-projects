CREATE TABLE [dbo].[RuleSetRuleOrder]
(
	RuleSetId int not null,
	RuleId int not null,
	ExecutionOrder int not null,
	RuleScript varchar(max) not null, 
	DateCreated datetime default(getdate()) not null,
	DateUpdated datetime null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_RuleSetRuleOrder] PRIMARY KEY ([RuleSetId], [RuleId], [ExecutionOrder])
)
