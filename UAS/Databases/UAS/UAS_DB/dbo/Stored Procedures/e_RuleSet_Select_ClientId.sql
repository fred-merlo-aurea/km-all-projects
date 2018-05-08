create procedure e_RuleSet_Select_ClientId
@clientId int
as
	begin
		select *
		from RuleSet with(nolock)
		where clientId = @clientId
	end
go
