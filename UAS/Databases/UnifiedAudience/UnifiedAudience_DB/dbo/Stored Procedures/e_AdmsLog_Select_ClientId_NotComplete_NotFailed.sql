create procedure e_AdmsLog_Select_ClientId_NotComplete_NotFailed
@clientId int
as
	begin
		set nocount on

		select *
		from AdmsLog with(nolock)
		where ClientId=@clientId
		and FileStatusId not in (
			select CodeId 
			from UAD_Lookup..Code with(nolock)
			where CodeTypeId = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'File Status') 
			and CodeName in ('Failed','Completed'))
	end
go
