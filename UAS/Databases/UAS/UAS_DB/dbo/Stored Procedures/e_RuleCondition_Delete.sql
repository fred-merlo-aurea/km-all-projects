create procedure e_RuleCondition_Delete
@ruleId int,
@lineNumber int
as
	begin
		set nocount on
		delete RuleCondition where RuleId = @ruleId and Line = @lineNumber
	end
go
