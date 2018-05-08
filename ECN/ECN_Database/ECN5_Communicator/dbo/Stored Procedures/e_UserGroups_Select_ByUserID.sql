CREATE PROCEDURE [dbo].[e_UserGroups_Select_ByUserID]  	
	@UserID int
AS     
BEGIN     
	Select ug.*
	FROM UserGroups ug with (nolock)
	WHERE ug.UserID = @UserID and ug.IsDeleted = 0 
END