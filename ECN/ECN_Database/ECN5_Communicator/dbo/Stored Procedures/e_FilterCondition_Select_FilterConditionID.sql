CREATE PROCEDURE [dbo].[e_FilterCondition_Select_FilterConditionID]   
@FilterConditionID int = NULL
AS
	SELECT fc.*, f.CustomerID
	FROM FilterCondition fc with (nolock) 
		join FilterGroup fg with (nolock) on fc.FilterGroupID = fg.FilterGroupID
		join Filter f with (nolock) on fg.FilterID = f.FilterID 
	WHERE 
		fc.FilterConditionID = @FilterConditionID and 
		f.IsDeleted = 0 and
		fg.IsDeleted = 0 and
		fc.IsDeleted = 0
