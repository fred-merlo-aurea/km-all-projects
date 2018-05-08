create procedure e_AdmsLog_Select_ClientId
@ClientId int
as
	begin
		set nocount on
		select * 
		from AdmsLog with(nolock)
		where ClientId = @ClientId
	end
go
