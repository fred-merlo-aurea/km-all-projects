CREATE PROCEDURE [dbo].[e_FilterCondition_Exists_ByID] 
	@FilterConditionID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS	(
				SELECT TOP 1 fc.FilterConditionID 
				FROM FilterCondition fc WITH (NOLOCK) join FilterGroup fg WITH (NOLOCK) on fc.FilterGroupID = fg.FilterGroupID join Filter f WITH (NOLOCK) on fg.FilterID = f.FilterID 
				WHERE f.CustomerID = @CustomerID AND fc.FilterConditionID = @FilterConditionID AND f.IsDeleted = 0 and fg.IsDeleted = 0 and fc.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0
END