CREATE proc [dbo].[e_ApplicationUsers_Save_IsApproved]
@UserID uniqueidentifier,
@IsApproved bit
as
BEGIN

	set nocount on

		update ApplicationUsers 
		set IsApproved = @IsApproved 
		where userID = @UserID

END