create procedure o_IsRuleNameUnique
@clientId int,
@ruleName varchar(250)
as
	begin
		select count(RuleId)
		from [Rule] with(nolock)
		where ClientId = @clientId and RuleName = @ruleName
	end
go
