CREATE PROCEDURE [dbo].[e_UserAction_Delete_UserID]  
	@UserID int	
AS
BEGIN
	DELETE FROM UserActions WHERE UserID = @UserID
END
