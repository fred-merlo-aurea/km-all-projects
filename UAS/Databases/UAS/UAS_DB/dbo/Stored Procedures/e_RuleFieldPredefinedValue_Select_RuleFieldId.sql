create procedure e_RuleFieldPredefinedValue_Select_RuleFieldId
@ruleFieldId int
as
	begin
		set nocount on
		
		select *
		from RuleFieldPredefinedValue with(nolock)
		where RuleFieldId = @ruleFieldId
		order by ItemOrder
	end
go
