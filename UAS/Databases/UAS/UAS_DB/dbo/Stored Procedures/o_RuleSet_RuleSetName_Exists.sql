create procedure o_RuleSet_RuleSetName_Exists
@ruleSetName varchar(250),
@clientId int
as
	begin
		select count(RuleSetId)
		from RuleSet with(nolock)
		where RuleSetName = @ruleSetName and ClientId = @clientId
	end
go
