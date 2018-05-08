CREATE  PROC [dbo].[e_FilterGroup_Delete_FilterID] 
(
	@CustomerID int = NULL,
	@FilterID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE fg SET fg.IsDeleted = 1, fg.UpdatedDate = GETDATE(), fg.UpdatedUserID = @UserID
	FROM FilterGroup fg
		JOIN Filter f WITH (NOLOCK) ON fg.FilterID = f.FilterID
	WHERE f.CustomerID = @CustomerID AND fg.FilterID = @FilterID
END
