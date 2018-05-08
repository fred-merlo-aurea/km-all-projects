create procedure e_DataCompareCostThirdParty_Select_ClientId
@clientId int
as
	begin
		set nocount on
		select * 
		from DataCompareCostThirdParty with(nolock)
		where ClientId = @clientId
	end
go
