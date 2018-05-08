create procedure e_RuleField_Select_ClientId_DataTable_Field_IsActive
@clientId int,
@dataTable varchar(75),
@field varchar(50),
@isActive bit
as
	begin
		set nocount on

		if(@dataTable in ('PubSubscriptions','Subscriptions'))
			set @clientId = 0

		select *
		from RuleField with(nolock)
		where ClientId = @clientId 
		and DataTable = @dataTable
		and Field = @field
		and IsActive = @isActive
	end
go

