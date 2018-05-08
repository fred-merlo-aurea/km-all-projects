
CREATE PROCEDURE [dbo].[e_User_UserName_Exists]
	@UserName varchar(100),
	@UserID int
AS
BEGIN
	if exists (Select top 1 * from [User] u where u.UserName = @UserName and u.UserID != @UserID)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN 
		SELECT 0
	END
END