CREATE PROCEDURE [dbo].[e_Select_DeptItemReferences_ItemID]  
@ItemID int
AS
SELECT 
	DeptItemRefID,
	DepartmentID,
	Item,
	ItemID,
	Share,
	CreatedDate,
	UpdatedDate 
FROM DeptItemReferences WITH(NOLOCK) WHERE ItemID = @ItemID
