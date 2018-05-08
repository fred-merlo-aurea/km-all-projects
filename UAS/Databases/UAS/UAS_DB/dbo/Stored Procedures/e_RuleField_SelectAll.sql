create procedure e_RuleField_SelectAll
as
	begin
		set nocount on

		select *
		from RuleField with(nolock)
	end
go
