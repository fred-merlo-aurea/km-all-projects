CREATE PROCEDURE [dbo].[e_ApplicationUsers_Select_UserName_Password]   
@UserName  varchar(100),
@Password  varchar(50)
AS
BEGIN

	set nocount on

	if exists(select username from ApplicationUsers with(nolock)  where username = @UserName and password = @Password)
		begin
			update ApplicationUsers 
			set FailedPasswordAttemptCount = null 
			where UserName = @UserName	
		end

	Select * from ApplicationUsers With(NoLock) 
	where UserName = @UserName and [Password] = @Password

END