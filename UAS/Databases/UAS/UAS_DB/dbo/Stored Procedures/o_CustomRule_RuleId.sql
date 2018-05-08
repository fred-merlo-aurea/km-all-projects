create procedure o_CustomRule_RuleId
@ruleId int
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

		where r.ruleId = @ruleId
		order by rsro.ExecutionOrder, cond.Line
	end
go
