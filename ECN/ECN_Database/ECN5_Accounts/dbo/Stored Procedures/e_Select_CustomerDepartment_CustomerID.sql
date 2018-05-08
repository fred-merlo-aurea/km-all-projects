CREATE PROCEDURE [dbo].[e_Select_CustomerDepartment_CustomerID]
	@customerID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT DepartmentID, CustomerID, DepartmentName, DepartmentDesc
    FROM CustomerDepartments WITH(NOLOCK) WHERE CustomerID = @customerID
END
