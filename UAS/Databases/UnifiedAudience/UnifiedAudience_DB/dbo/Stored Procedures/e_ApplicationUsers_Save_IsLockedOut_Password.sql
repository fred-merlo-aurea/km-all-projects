create proc [dbo].[e_ApplicationUsers_Save_IsLockedOut_Password]
@Password varchar(50),
@UserID uniqueidentifier
as
BEGIN

	set nocount on

	update ApplicationUsers 
	set [password] = @Password, IsLockedOut = 0, LastPasswordChangedDate = GETDATE() 
	where userID = @UserID

END