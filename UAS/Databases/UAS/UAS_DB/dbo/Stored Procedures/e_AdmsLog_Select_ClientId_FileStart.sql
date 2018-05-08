create procedure e_AdmsLog_Select_ClientId_FileStart
@ClientId int,
@FileStart date
as
	begin
		set nocount on
		select * 
		from AdmsLog with(nolock)
		where ClientId = @ClientId
		and cast(FileStart as date) = @FileStart
	end
go
