create procedure job_Rule_Delete
@ruleSetId int,
@ruleId int,
@userId int
as
	begin
		set nocount on
		
		update [Rule]
		set IsActive='false',UpdatedByUserId=@userId, DateUpdated = getdate()
		where RuleId = @ruleId

		update RuleCondition
		set IsActive='false',UpdatedByUserId=@userId
		where RuleId = @ruleId

		update RuleResult
		set IsActive='false',UpdatedByUserId=@userId
		where RuleId = @ruleId

		update RuleSet_Rule_Map
		set IsActive='false',UpdatedByUserId=@userId
		where RuleId = @ruleId and RuleSetId = @ruleSetId

		declare @cEO int = (select ExecutionOrder from RuleSetRuleOrder with(nolock) where RuleId = @ruleId and RuleSetId = @ruleSetId)

		delete RuleSetRuleOrder
		where RuleId = @ruleId and RuleSetId = @ruleSetId

		update RuleSetRuleOrder
		set ExecutionOrder = ExecutionOrder -1, DateUpdated=getdate(), UpdatedByUserId=@userId
		where RuleSetId = @ruleSetId and ExecutionOrder > @cEO

	end
go
