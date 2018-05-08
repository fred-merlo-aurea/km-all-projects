CREATE PROCEDURE [dbo].[e_Select_DeptItemReferences]  
AS
SELECT 
	DeptItemRefID,
	DepartmentID,
	Item,
	ItemID,
	Share,
	CreatedDate,
	UpdatedDate 
FROM DeptItemReferences WITH(NOLOCK)
