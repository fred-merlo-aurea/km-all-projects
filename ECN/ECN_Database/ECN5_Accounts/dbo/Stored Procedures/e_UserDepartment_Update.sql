CREATE PROCEDURE [dbo].[e_UserDepartment_Update] 
	@UserDepartmentID int, 
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
		@UserDepartmentID = @UserDepartmentID
END
