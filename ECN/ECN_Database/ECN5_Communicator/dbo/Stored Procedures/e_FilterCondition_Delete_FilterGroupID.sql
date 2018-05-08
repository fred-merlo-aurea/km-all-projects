CREATE  PROC [dbo].[e_FilterCondition_Delete_FilterGroupID] 
(
	@CustomerID int = NULL,
	@FilterGroupID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE fc SET fc.IsDeleted = 1, fc.UpdatedDate = GETDATE(), fc.UpdatedUserID = @UserID
	FROM FilterCondition fc
		JOIN FilterGroup fg with (nolock) on fc.FilterGroupID = fg.FilterGroupID
		JOIN Filter f WITH (NOLOCK) ON fg.FilterID = f.FilterID
	WHERE f.CustomerID = @CustomerID AND fc.FilterGroupID = @FilterGroupID
END
