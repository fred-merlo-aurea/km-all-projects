create procedure e_RuleValue_Select_IsActive
@isActive bit = 'true'
as
	begin
		select distinct rv.*,isnull(rvC.CodeName,'NotSet') as '_ruleValueTypeEnum',m.RuleId
		from RuleValue rv with(nolock)
		join RuleSet_Rule_Map m with(nolock) on m.RuleValueId = rv.RuleValueId
		left join UAD_LOOKUP..Code rvC with(nolock) on rv.RuleValueTypeId = rvC.CodeId
		where rv.IsActive = @isActive
	end
go
