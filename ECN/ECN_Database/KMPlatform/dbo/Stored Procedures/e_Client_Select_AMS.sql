create procedure e_Client_Select_AMS
@IsAms bit = 'true',
@IsActive bit = 'true'
as
	begin
		set nocount on
		select * 
		from Client with(nolock)
		where IsAMS = @IsAms
		and IsActive = @IsActive
	end
go
