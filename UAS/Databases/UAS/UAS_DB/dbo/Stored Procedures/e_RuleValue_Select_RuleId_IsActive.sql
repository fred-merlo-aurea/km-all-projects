create procedure e_RuleValue_Select_RuleId_IsActive
@ruleId int,
@isActive bit = 'true'
as
	begin
		set nocount on

		select distinct rv.*,isnull(rvC.CodeName,'NotSet') as '_ruleValueTypeEnum',m.RuleId
		from RuleValue rv with(nolock)
		join RuleSet_Rule_Map m with(nolock) on m.RuleValueId = rv.RuleValueId
		left join UAD_LOOKUP..Code rvC with(nolock) on rv.RuleValueTypeId = rvC.CodeId
		where m.RuleId = @ruleId and rv.IsActive = @isActive
	end
go