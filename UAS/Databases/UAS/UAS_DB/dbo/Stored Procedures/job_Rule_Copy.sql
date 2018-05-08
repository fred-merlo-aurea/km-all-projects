create  procedure job_Rule_Copy
@existingRuleId int,
@newRuleSetId int,
@createdByUserId int
as
	begin
		set nocount on

		declare @newRuleId int
		--Rule
		insert into [Rule] (RuleName,DisplayName,RuleDescription,IsDeleteRule,IsSystem,IsGlobal,ClientId,IsActive,DateCreated,CreatedByUserId,CustomImportRuleId,RuleActionId)
				
		select RuleName + ' copy ' + REPLACE(CONVERT (CHAR(10), getdate(), 101),'/',''),
		DisplayName,RuleDescription,IsDeleteRule,IsSystem,IsGlobal,ClientId,IsActive,getdate(),createdByUserId,CustomImportRuleId,RuleActionId
		from [Rule] with(nolock)
		where RuleId = @existingRuleId
		set @newRuleId = @@IDENTITY;

		--RuleCondition
		insert into RuleCondition (RuleId,Line,IsGrouped,GroupNumber,ChainId,CompareField,CompareFieldPrefix,IsClientField,OperatorId,CompareValue,IsActive,CreatedDate,CreatedByUserId)
		select @newRuleId,Line,IsGrouped,GroupNumber,ChainId,CompareField,CompareFieldPrefix,IsClientField,OperatorId,CompareValue,IsActive,getdate(),@createdByUserId
		from RuleCondition rc with(nolock)
		where rc.RuleId = @existingRuleId

		--RuleResult
		insert into RuleResult (RuleId,UpdateField,UpdateFieldPrefix,UpdateFieldValue,IsClientField,IsActive,CreatedDate,CreatedByUserId)
		select @newRuleId,UpdateField,UpdateFieldPrefix,UpdateFieldValue,IsClientField,IsActive,getdate(),@createdByUserId
		from RuleResult rr with(nolock)
		where rr.RuleId = @existingRuleId

		--RuleSet_Rule_Map - should not be any entries here as this is for ADMS engine rules

		--RuleSetRuleOrder
		declare @eo int = (select max(isnull(ExecutionOrder,0)) from RuleSetRuleOrder where RuleSetId = @newRuleSetId)

		insert into RuleSetRuleOrder (RuleSetId,RuleId,ExecutionOrder,RuleScript,DateCreated,CreatedByUserId)
		select top 1 @newRuleSetId,@newRuleId,@eo + 1,RuleScript,getdate(),@createdByUserId
		from RuleSetRuleOrder rsro 
		where rsro.RuleId = @existingRuleId

		select @newRuleId
	end
go