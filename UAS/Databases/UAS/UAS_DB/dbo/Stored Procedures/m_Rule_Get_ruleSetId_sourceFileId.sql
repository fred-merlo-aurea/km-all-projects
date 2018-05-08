create procedure m_Rule_Get_ruleSetId_sourceFileId
@ruleSetId int,
@sourceFileId int
as
	begin
		set nocount on

		select r.ruleId, r.ruleName, 
		isnull(rt.DisplayName,'') as 'ruleType',
		isnull(ra.DisplayName,'') as 'ruleAction',
		o.ExecutionOrder as 'sortOrder',
		r.IsGlobal as 'isTemplateRule',
		o.RuleScript as 'ruleScript',
		o.ruleSetId,
		@sourceFileId as 'sourceFileId'
		from [Rule] r with(nolock)
		join UAD_Lookup..Code rt with(nolock) on r.CustomImportRuleId = rt.CodeId
		join UAD_Lookup..Code ra with(nolock) on r.RuleActionId = ra.CodeId
		join RuleSetRuleOrder o with(nolock) on r.RuleId = o.RuleId
		where o.RuleSetId = @ruleSetId and r.IsActive = 'true'
	end
go
