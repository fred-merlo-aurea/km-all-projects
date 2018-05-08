CREATE PROCEDURE [dbo].[e_UserDepartment_Delete_UserID] 
	@UserID int		
AS
BEGIN	
	SET NOCOUNT OFF;

    DELETE FROM UserDepartments WHERE UserID = @UserID
END
