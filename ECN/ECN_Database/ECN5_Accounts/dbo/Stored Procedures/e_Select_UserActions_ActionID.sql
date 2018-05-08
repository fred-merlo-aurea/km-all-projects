CREATE PROCEDURE [dbo].[e_Select_UserActions_ActionID] 
@ActionID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		UserActionID,
		UserID,
		ActionID,
		Active	         
	FROM 
		UserActions WITH (NOLOCK) where ActionID = @ActionID
END
