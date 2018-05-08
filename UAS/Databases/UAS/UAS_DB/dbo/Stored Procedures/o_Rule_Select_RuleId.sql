create procedure o_Rule_Select_RuleId
@ruleId int
as
	begin
		set nocount on
		select  r.RuleId, r.RuleName, r.DisplayName, r.RuleDescription, r.IsDeleteRule, r.IsSystem, r.ClientId, r.IsActive, 
				rm.RuleSetId, rm.RecordTypeId, rm.RuleValueId, rm.FreeTextValue, 
				
				rm.ConditionGroup,rm.ConditionChainId,rm.ConditionOrder,rm.ConditionBreakResult,
				rm.RuleOrder,rm.RuleChainId,
				rm.RuleGroup,rm.RuleGroupChainId,rm.RuleGroupOrder,rm.RuleGroupBreakResult,
				
				isnull(rtC.CodeName,'NotSet') as '_recordTypeEnum'
		from [Rule] r with(nolock)
		join RuleSet_Rule_Map rm with(nolock) on r.RuleId = rm.RuleId
		left join UAD_LOOKUP..Code rtC with(nolock) on rm.RecordTypeId = rtC.CodeId
		where r.RuleId = @ruleId
		order by rm.ConditionGroup,rm.ConditionOrder,rm.RuleOrder
	end
go
