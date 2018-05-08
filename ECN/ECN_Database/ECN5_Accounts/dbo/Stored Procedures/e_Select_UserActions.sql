CREATE PROCEDURE [dbo].[e_Select_UserActions]    
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
