CREATE PROCEDURE [dbo].[e_Select_Users_DepartmentID]
	@DepartmentID int		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT ud.*, cd.CustomerID 
    from UserDepartments ud with(nolock) 
		join CustomerDepartments cd with (nolock) on ud.DepartmentID = cd.DepartmentID 
	where ud.DepartmentID = @DepartmentID
END
