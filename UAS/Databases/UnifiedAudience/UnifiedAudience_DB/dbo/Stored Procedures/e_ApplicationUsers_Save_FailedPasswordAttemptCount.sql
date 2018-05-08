CREATE proc [dbo].[e_ApplicationUsers_Save_FailedPasswordAttemptCount]
@UserName varchar(100)
as
BEGIN

	set nocount on

	if exists(select username from ApplicationUsers with(nolock)  where username = @UserName)
		begin
		
			declare @FailedPasswordAttemptCount int
			
			select @FailedPasswordAttemptCount = ISNULL(FailedPasswordAttemptCount,0) from ApplicationUsers where UserName = @UserName
			
			if @FailedPasswordAttemptCount = 4 
				begin
					update ApplicationUsers 
					set FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1,
						IsLockedOut = 1,
						LastLockOutDate = GETDATE()					 
					where UserName = @UserName		
				
					select @FailedPasswordAttemptCount+1	
				end
			else if @FailedPasswordAttemptCount > 4
				begin
					select @FailedPasswordAttemptCount	
				end  
			else
				begin
					update ApplicationUsers 
					set FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1 
					where UserName = @UserName
					
					select @FailedPasswordAttemptCount+1	
				end	
		end	
	else
		Begin
			select 0	
		End

END