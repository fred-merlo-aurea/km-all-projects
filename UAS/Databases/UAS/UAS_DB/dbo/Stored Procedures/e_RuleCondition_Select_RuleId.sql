create procedure e_RuleCondition_Select_RuleId
@ruleId int
as
	begin
		set nocount on
		select * 
		from RuleCondition with(nolock)
		where RuleId = @ruleId
	end
go
