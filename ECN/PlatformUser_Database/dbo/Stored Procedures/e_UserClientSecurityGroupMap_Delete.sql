
CREATE PROCEDURE e_UserClientSecurityGroupMap_Delete
	@UserClientSecurityGroupMapID int
AS
BEGIN
	DELETE FROM UserClientSecurityGroupMap
	WHERE UserClientSecurityGroupMapID = @UserClientSecurityGroupMapID
END
