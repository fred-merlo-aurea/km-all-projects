create proc [dbo].[e_ApplicationUsers_Save_TrialExpireDate_IsApproved]
@UserID uniqueidentifier,
@TrialExpireDate int,
@IsApproved bit
as
BEGIN

	set nocount on

	update ApplicationUsers 
	set TrialExpireDate = @TrialExpireDate, IsApproved = @IsApproved 
	where userID = @UserID

END