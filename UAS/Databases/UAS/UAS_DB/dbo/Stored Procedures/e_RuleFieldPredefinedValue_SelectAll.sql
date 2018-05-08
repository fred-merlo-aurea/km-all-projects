create procedure e_RuleFieldPredefinedValue_SelectAll
as
	begin
		set nocount on

		select *
		from RuleFieldPredefinedValue with(nolock)
	end
go
