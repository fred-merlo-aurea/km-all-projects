CREATE PROCEDURE [dbo].[e_UserDepartment_Update_UserID] 
	@UserID int,
	@IsDefaultDept bit,
	@DepartmentID int
AS
BEGIN
	UPDATE UserDepartments 
	SET
		UserID = @UserID,
		DepartmentID = @DepartmentID, 
		IsDefaultDept = @IsDefaultDept
	WHERE
		UserID = @UserID
END
