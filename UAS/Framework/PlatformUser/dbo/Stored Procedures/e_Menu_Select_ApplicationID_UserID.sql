CREATE PROCEDURE [dbo].[e_Menu_Select_ApplicationID_UserID]
	@ApplicationID int,
	@UserID int
AS
	SELECT m.*
	FROM Menu m with(nolock)
	JOIN [User] u on u.UserID = @UserID
	JOIN MenuSecurityGroupMap msgm on msgm.MenuID = m.MenuID
	JOIN UserClientSecurityGroupMap usgm on usgm.UserID = u.UserID and msgm.SecurityGroupID = usgm.SecurityGroupID and usgm.ClientID = u.DefaultClientID
	WHERE ApplicationID = @ApplicationID
	and msgm.HasAccess = 1
RETURN 0
