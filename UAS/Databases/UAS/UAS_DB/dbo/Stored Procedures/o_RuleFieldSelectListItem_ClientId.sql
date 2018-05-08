create procedure o_RuleFieldSelectListItem_ClientId
@clientId int
as
	begin
		set nocount on
		select rf.RuleFieldId,rf.DataTable,rf.Field,rf.UIControl,rf.IsMultiSelect
		from RuleField rf with(nolock)
		where rf.clientId in (0,@clientId)
		and rf.IsActive = 'true'
	end
go
