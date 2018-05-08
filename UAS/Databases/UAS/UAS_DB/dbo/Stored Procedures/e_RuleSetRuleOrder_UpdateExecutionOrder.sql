create procedure e_RuleSetRuleOrder_UpdateExecutionOrder
@RuleSetId	int,
@RuleId	int,
@ExecutionOrder	int
as
	begin
		set nocount on
		declare @replacedEO int = (select ExecutionOrder from RuleSetRuleOrder with(nolock) where RuleSetId = @RuleSetId and RuleId = @RuleId)

		update RuleSetRuleOrder
		set ExecutionOrder = @replacedEO,
			DateUpdated = getdate()
		where RuleSetId = @RuleSetId and ExecutionOrder = @ExecutionOrder

		update RuleSetRuleOrder
		set ExecutionOrder = @ExecutionOrder,
			DateUpdated = getdate()
		where RuleSetId = @RuleSetId and RuleId = @RuleId
	end
go

