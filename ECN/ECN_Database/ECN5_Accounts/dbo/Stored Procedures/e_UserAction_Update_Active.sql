CREATE PROCEDURE [dbo].[e_UserAction_Update_Active] 
	@UserActionID int,   	
	@Active char(2) 
AS
BEGIN
	UPDATE UserActions SET Active = @Active WHERE UserActionID = @UserActionID
END
