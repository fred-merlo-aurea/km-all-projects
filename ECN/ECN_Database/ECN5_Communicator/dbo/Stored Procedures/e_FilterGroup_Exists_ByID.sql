CREATE PROCEDURE [dbo].[e_FilterGroup_Exists_ByID] 
	@FilterGroupID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS	(
				SELECT TOP 1 fg.FilterGroupID 
				FROM FilterGroup fg WITH (NOLOCK) join Filter f WITH (NOLOCK) on fg.FilterID = f.FilterID 
				WHERE f.CustomerID = @CustomerID AND fg.FilterGroupID = @FilterGroupID AND f.IsDeleted = 0 and fg.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0
END