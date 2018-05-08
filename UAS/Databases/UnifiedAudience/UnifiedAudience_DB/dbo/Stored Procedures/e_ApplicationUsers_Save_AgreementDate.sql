CREATE proc [dbo].[e_ApplicationUsers_Save_AgreementDate]
@UserID uniqueidentifier
as
BEGIN

	set nocount on

		update ApplicationUsers 
		set AgreementDate = GETDATE() 
		where userID = @UserID

END