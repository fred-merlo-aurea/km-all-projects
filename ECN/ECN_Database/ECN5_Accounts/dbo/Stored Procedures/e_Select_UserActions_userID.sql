CREATE PROCEDURE [dbo].[e_Select_UserActions_userID] 
@userID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		ua.*, u.CustomerID	         
	FROM 
		UserActions ua WITH (NOLOCK) 
		join Users u WITH (NOLOCK) on ua.UserID = u.UserID
	where 
		ua.UserID = @userID
END
