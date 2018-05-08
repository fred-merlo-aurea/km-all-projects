create procedure o_RuleFieldNeedValue
as
	begin
		set nocount on
		------this will run to get list of items that need values - pass this to a sproc on Client DB
		select rf.RuleFieldId,rf.ClientId,rf.DataTable,rf.Field
		from RuleField rf with(nolock)
		left join RuleFieldPredefinedValue pv with(nolock) on rf.RuleFieldId = pv.RuleFieldId
		where rf.clientid !=0 
		and rf.uicontrol = 'dropdownlist' 
		and pv.RuleFieldId is null
		order by rf.RuleFieldId
	end
go
