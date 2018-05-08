create proc [dbo].[e_ApplicationUsers_Save_RoleID_IsApproved]
@UserID uniqueidentifier,
@RoleID int,
@IsApproved bit
as
BEGIN

	set nocount on

		update ApplicationUsers 
		set RoleID = @RoleID, IsApproved = @IsApproved 
		where userID = @UserID

END