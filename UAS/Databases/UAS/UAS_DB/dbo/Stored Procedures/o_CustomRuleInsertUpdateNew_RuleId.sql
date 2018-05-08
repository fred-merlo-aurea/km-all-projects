create procedure o_CustomRuleInsertUpdateNew_RuleId
@ruleId int
as
	begin
		set nocount on

		select rf.DataTable + '.' + rf.Field as 'dataTableColumnName',
		rf.IsClientField,
		rf.UIControl,
		rf.DataType,
		rf.RuleFieldId,
		rf.IsMultiSelect,
		rr.UpdateFieldValue as 'updateText',
		rr.UpdateFieldValue as 'updateValue'

		from RuleResult rr with(nolock)
		join RuleField rf with(nolock) on rr.RuleFieldId = rf.RuleFieldId
		where rr.RuleId = @ruleId 
	end
go

