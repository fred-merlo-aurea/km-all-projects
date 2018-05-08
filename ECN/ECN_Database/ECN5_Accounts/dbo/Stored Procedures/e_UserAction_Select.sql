-- Procedure
CREATE PROCEDURE [dbo].[e_UserAction_Select]    
(
	@UserID int
)
AS  

BEGIN   
 SET NOCOUNT ON;  
  
    SELECT	UserActionID,
			UserID,
			ActionID,
			Active            
	FROM   
		UserActions WITH (NOLOCK)
	Where
		UserID = @UserID   
END
