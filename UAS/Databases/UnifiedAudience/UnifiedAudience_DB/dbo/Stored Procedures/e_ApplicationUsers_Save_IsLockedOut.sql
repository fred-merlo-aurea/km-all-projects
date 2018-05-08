CREATE proc [dbo].[e_ApplicationUsers_Save_IsLockedOut]
@UserID uniqueidentifier,
@IsLockedOut bit
as
BEGIN

	set nocount on

	if @IsLockedOut = 0
		begin
			update ApplicationUsers 
			set 
				IsLockedOut = 0,
				FailedPasswordAttemptCount = null
			where 
				userID = @UserID
		end
	else
		begin
			update ApplicationUsers 
			set 
				IsLockedOut = 1,
				LastLockOutDate = GETDATE()
			where 
				userID = @UserID	
		end

END