CREATE PROCEDURE [dbo].[e_UserDepartment_Insert] 
	@UserID int, 
	@DepartmentID int,
	@IsDefaultDept bit 	
AS
BEGIN
	INSERT INTO UserDepartments(UserId, DepartmentID, IsDefaultDept) 
		VALUES (@UserID, @DepartmentID, @IsDefaultDept)  
END
