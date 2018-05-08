create procedure e_UserAuthorizationLog_LogOut
@userAuthLogId int
AS
	update UserAuthorizationLog 
	set LogOutDate = GETDATE(),
		LogOutTime = GETDATE()
	where UserAuthLogID = @userAuthLogId
go
