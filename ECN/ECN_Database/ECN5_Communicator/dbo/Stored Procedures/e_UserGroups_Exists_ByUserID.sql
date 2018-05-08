CREATE PROCEDURE [dbo].[e_UserGroups_Exists_ByUserID] 
	@UserID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 UGID FROM UserGroups WITH (NOLOCK) WHERE UserID = @UserID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END