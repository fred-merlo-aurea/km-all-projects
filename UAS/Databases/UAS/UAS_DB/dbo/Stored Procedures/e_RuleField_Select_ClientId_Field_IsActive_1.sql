create procedure e_RuleField_Select_ClientId_Field_IsActive
@clientId int,
@field varchar(50),
@isActive bit
as
	begin
		set nocount on

		select *
		from RuleField with(nolock)
		where ClientId = @clientId 
		and Field = @field
		and IsActive = @isActive
	end
go
