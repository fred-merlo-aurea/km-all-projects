CREATE PROCEDURE [dbo].[e_UserAction_Insert] 
	@UserID int,
	@ActionID int, 
	@Active char(1) 
AS
BEGIN
	INSERT INTO UserActions(UserID, ActionID, Active)  
	VALUES (@UserID, @ActionID, @Active)  
END
