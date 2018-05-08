create procedure e_Rule_Select_RuleId
@ruleId int
as
	begin
		set nocount on
		select * 
		from [Rule] with(nolock)
		where RuleId = @ruleId
	end
go
