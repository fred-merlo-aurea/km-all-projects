CREATE PROCEDURE [dbo].[e_UserGroups_Insert]  	
	@UserID int,
	@GroupID int
AS     
BEGIN     
	INSERT INTO UserGroups (UserID, GroupID) VALUES (@userID, @GroupID)    
END
