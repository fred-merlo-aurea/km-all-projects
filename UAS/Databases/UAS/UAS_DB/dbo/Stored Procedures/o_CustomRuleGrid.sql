create procedure o_CustomRuleGrid
as
	begin
		set nocount on

		select rsro.RuleSetId,
			   rsro.RuleId,
			   r.RuleName,
			   rc.DisplayName + ' - ' + ac.DisplayName as 'RuleTypeAction',
			   rsro.ExecutionOrder,
			   rsro.RuleScript
		from RuleSetRuleOrder rsro with(nolock)
		join [Rule] r with(nolock) on rsro.RuleId = r.RuleId
		join UAD_Lookup..Code rC with(nolock) on r.CustomImportRuleId = rc.CodeId
		join UAD_Lookup..Code aC with(nolock) on r.RuleActionId = ac.CodeId
		order by rsro.ExecutionOrder
	end
go
