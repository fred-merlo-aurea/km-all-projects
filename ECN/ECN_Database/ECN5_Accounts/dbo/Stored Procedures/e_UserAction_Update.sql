CREATE PROCEDURE [dbo].[e_UserAction_Update] 
	@UserActionID int, 
	@UserID int,
	@ActionID int, 
	@Active char(2) 
AS
BEGIN
	UPDATE UserActions 
	SET
		UserID = @UserID,
		ActionID = @ActionID, 
		Active = @Active
	WHERE
		UserActionID = @UserActionID
END
