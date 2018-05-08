create procedure e_RuleField_Select_RuleFieldId
@ruleFieldId int
as
	begin
		set nocount on

		select *
		from RuleField with(nolock)
		where RuleFieldId = @ruleFieldId 
	end
go
