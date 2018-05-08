create procedure e_AdmsLog_Select_ClientId_FileNameExact
@ClientId int,
@FileNameExact varchar(260)
as
	begin
		set nocount on
		select * 
		from AdmsLog with(nolock)
		where ClientId = @ClientId
		and FileNameExact = @FileNameExact
	end
go