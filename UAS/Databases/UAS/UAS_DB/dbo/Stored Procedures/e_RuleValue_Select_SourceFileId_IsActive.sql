create procedure e_RuleValue_Select_SourceFileId_IsActive
@sourceFileId int,
@IsActive bit
as
	begin
		set nocount on

		select distinct rv.*,isnull(rvC.CodeName,'NotSet') as '_ruleValueTypeEnum',m.RuleId
		from RuleValue rv with(nolock)
		join RuleSet_Rule_Map m with(nolock) on m.RuleValueId = rv.RuleValueId
		join RuleSet_File_Map f with(nolock) on m.RuleSetId = f.RuleSetId
		left join UAD_LOOKUP..Code rvC with(nolock) on rv.RuleValueTypeId = rvC.CodeId
		where rv.IsActive = @isActive 
		and m.IsActive = @isActive
		and f.IsActive = @isActive
		and f.SourceFileId = @sourceFileId
	end
go
