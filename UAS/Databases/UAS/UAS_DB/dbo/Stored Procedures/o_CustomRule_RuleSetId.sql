create procedure o_CustomRule_RuleSetId
@ruleSetId int
as
	begin
		set nocount on

		select rsro.RuleSetId,
			   rsro.RuleId,
			   --rc.DisplayName + ' - ' + ac.DisplayName as 'RuleTypeAction',--CustomImportRule_Code_DisplayName - RuleAction_Code_DisplayName
			   r.RuleName,
			   rc.DisplayName as 'CustomImportRuleDisplayName',
			   ac.DisplayName as 'RuleActionDisplayName',

			   isnull(chainCode.DisplayName, '') as 'Connector',
			   cond.CompareField as 'DatabaseField',
			   operatorCode.DisplayName as 'Operator',
			   cond.CompareValue as 'Values',
			   rsro.ExecutionOrder

		from RuleSetRuleOrder rsro with(nolock)
		join RuleCondition cond with(nolock) on rsro.RuleId = cond.RuleId
		join [Rule] r with(nolock) on rsro.RuleId = r.RuleId
		join UAD_Lookup..Code rC with(nolock) on r.CustomImportRuleId = rc.CodeId
		join UAD_Lookup..Code aC with(nolock) on r.RuleActionId = ac.CodeId
		left join UAD_Lookup..Code chainCode with(nolock) on cond.ChainId = chainCode.CodeId
		join UAD_Lookup..Code operatorCode with(nolock) on cond.OperatorId = operatorCode.CodeId

		where rsro.ruleSetId = @ruleSetId
		order by rsro.ExecutionOrder, cond.Line

		----version 2
		--		select rsro.RuleSetId,
		--	   rsro.RuleId,
		--	   r.RuleName,
		--	   rc.DisplayName + ' - ' + ac.DisplayName as 'RuleTypeAction',
		--	   rsro.ExecutionOrder,
		--	   case when rr.RuleId is null then rsro.RuleScript else
		--	   'update ' + rr.UpdateFieldPrefix + 
		--	   ' set ' + rr.UpdateFieldPrefix + '.' + rr.UpdateField + ' = ''' + rr.UpdateFieldValue + '''' + 
		--	   ' from ' + rf.DataTable + ' ' + rsro.RuleScript
		--	   end as 'RuleScript'
		--from RuleSetRuleOrder rsro with(nolock)
		--join [Rule] r with(nolock) on rsro.RuleId = r.RuleId
		--join UAD_Lookup..Code rC with(nolock) on r.CustomImportRuleId = rc.CodeId
		--join UAD_Lookup..Code aC with(nolock) on r.RuleActionId = ac.CodeId
		--left join [RuleResult] rr with(nolock) on r.RuleId = rr.RuleId
		--left join [RuleField] rf with(nolock) on rr.RuleFieldId = rf.RuleFieldId 
		--where rsro.ruleSetId = @ruleSetId
		--order by rsro.ExecutionOrder

		----version 1
		--select rsro.RuleSetId,
		--	   rsro.RuleId,
		--	   r.RuleName,
		--	   rc.DisplayName + ' - ' + ac.DisplayName as 'RuleTypeAction',
		--	   rsro.ExecutionOrder,
		--	   rsro.RuleScript
		--from RuleSetRuleOrder rsro with(nolock)
		--join [Rule] r with(nolock) on rsro.RuleId = r.RuleId
		--join UAD_Lookup..Code rC with(nolock) on r.CustomImportRuleId = rc.CodeId
		--join UAD_Lookup..Code aC with(nolock) on r.RuleActionId = ac.CodeId
		--where rsro.ruleSetId = @ruleSetId
		--order by rsro.ExecutionOrder
	end
go
