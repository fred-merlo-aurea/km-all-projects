create procedure job_RuleSet_Copy
@existingRuleSetId int,
@newRuleSetId int,
@createdByUserId int
as
	begin
		set nocount on

		declare @existingRuleId int,
				@RuleName varchar(250),
				@DisplayName varchar(250),
				@RuleDescription varchar(1250),
				@IsDeleteRule bit,
				@IsSystem bit,
				@IsGlobal bit,
				@ClientId int,
				@IsActive bit,
				@CustomImportRuleId int,
				@RuleActionId int

		declare c cursor
		for 
			select r.RuleId,r.RuleName,r.DisplayName,r.RuleDescription,r.IsDeleteRule,r.IsSystem,r.IsGlobal,r.ClientId,r.IsActive,r.CustomImportRuleId,r.RuleActionId
			from [Rule] r with(nolock)
			join RuleSetRuleOrder rsro with(nolock) on r.RuleId = rsro.RuleId
			where rsro.RuleSetId = @existingRuleSetId
		open c
		fetch next from c into @existingRuleId, @RuleName,@DisplayName,@RuleDescription,@IsDeleteRule,@IsSystem,@IsGlobal,@ClientId,@IsActive,@CustomImportRuleId,@RuleActionId
		while @@FETCH_STATUS = 0
		begin
			declare @newRuleId int

			--Rule
			insert into [Rule] (RuleName,DisplayName,RuleDescription,IsDeleteRule,IsSystem,IsGlobal,ClientId,IsActive,DateCreated,CreatedByUserId,CustomImportRuleId,RuleActionId)
			values(@RuleName + ' copy ' + REPLACE(CONVERT (CHAR(10), getdate(), 101),'/',''),
				  @DisplayName,@RuleDescription,@IsDeleteRule,@IsSystem,@IsGlobal,@ClientId,@IsActive,getdate(),@createdByUserId,@CustomImportRuleId,@RuleActionId)
			set @newRuleId = @@IDENTITY;

			--RuleCondition
			insert into RuleCondition (RuleId,Line,IsGrouped,GroupNumber,ChainId,CompareField,CompareFieldPrefix,IsClientField,OperatorId,CompareValue,IsActive,CreatedDate,CreatedByUserId)
			select @newRuleId,Line,IsGrouped,GroupNumber,ChainId,CompareField,CompareFieldPrefix,IsClientField,OperatorId,CompareValue,IsActive,getdate(),@createdByUserId
			from RuleCondition rc with(nolock)
			join RuleSetRuleOrder rsro with(nolock) on rc.RuleId = rsro.RuleId
			where rsro.RuleSetId = @existingRuleSetId and rc.RuleId = @existingRuleId

			--RuleResult
			insert into RuleResult (RuleId,UpdateField,UpdateFieldPrefix,UpdateFieldValue,IsClientField,IsActive,CreatedDate,CreatedByUserId)
			select @newRuleId,UpdateField,UpdateFieldPrefix,UpdateFieldValue,IsClientField,IsActive,getdate(),@createdByUserId
			from RuleResult rr with(nolock)
			join RuleSetRuleOrder rsro with(nolock) on rr.RuleId = rsro.RuleId
			where rsro.RuleSetId = @existingRuleSetId  and rr.RuleId = @existingRuleId

			--RuleSet_Rule_Map - should not be any entries here as this is for ADMS engine rules
			insert into RuleSet_Rule_Map (RuleSetId,RuleId,RecordTypeId,RuleValueId,FreeTextValue,IsActive,ConditionGroup,ConditionChainId,ConditionOrder,ConditionBreakResult,RuleOrder,
											RuleChainId,RuleGroup,RuleGroupChainId,RuleGroupOrder,RuleGroupBreakResult,DateCreated,CreatedByUserId)
			select @newRuleSetId,@newRuleId,m.RecordTypeId,m.RuleValueId,m.FreeTextValue,m.IsActive,m.ConditionGroup,m.ConditionChainId,m.ConditionOrder,m.ConditionBreakResult,m.RuleOrder,
											m.RuleChainId,m.RuleGroup,m.RuleGroupChainId,m.RuleGroupOrder,m.RuleGroupBreakResult,getdate(),@createdByUserId
			from RuleSet_Rule_Map m
			join RuleSetRuleOrder rsro with(nolock) on m.RuleId = rsro.RuleId and m.RuleSetId = rsro.RuleSetId
			where rsro.RuleSetId = @existingRuleSetId and m.RuleId = @existingRuleId

			--RuleSetRuleOrder
			insert into RuleSetRuleOrder (RuleSetId,RuleId,ExecutionOrder,RuleScript,DateCreated,CreatedByUserId)
			select @newRuleSetId,@newRuleId,ExecutionOrder,RuleScript,getdate(),@createdByUserId
			from RuleSetRuleOrder rsro 
			where rsro.RuleSetId = @existingRuleSetId and rsro.RuleId = @existingRuleId

			fetch next from c into @existingRuleId,@RuleName,@DisplayName,@RuleDescription,@IsDeleteRule,@IsSystem,@IsGlobal,@ClientId,@IsActive,@CustomImportRuleId,@RuleActionId
		end
		close c
		deallocate c
	end
go
