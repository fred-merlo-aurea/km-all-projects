CREATE PROCEDURE [dbo].[e_Client_Select_UserID]
@UserID int
AS
Begin
	SELECT c.*
	FROM 
			[User] u With(NoLock)
			join [UserClientSecurityGroupMap] ucsgm With(NoLock) on u.UserID = ucsgm.UserID
			join [Client] c with (nolock) on c.ClientID = ucsgm.ClientID
	WHERE u.UserID = @UserID and u.IsActive = 1 and ucsgm.IsActive = 1 and c.IsActive = 1
End