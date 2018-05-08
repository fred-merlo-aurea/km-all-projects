CREATE PROCEDURE [dbo].[e_Select_UserDepartment_UserID]
	@userID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT ud.*, cd.CustomerID
    from UserDepartments ud with(nolock) 
		join CustomerDepartments cd with (nolock) on ud.DepartmentID = cd.DepartmentID 
    WHERE ud.UserID = @userID 		
END
