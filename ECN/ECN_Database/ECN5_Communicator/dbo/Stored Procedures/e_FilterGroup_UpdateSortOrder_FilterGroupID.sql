CREATE  PROC [dbo].[e_FilterGroup_UpdateSortOrder_FilterGroupID] 
(
	@UserID int = NULL,
    @FilterGroupID int = NULL,
    @SortOrder int = NULL
)
AS 
BEGIN
	Update FilterGroup SET SortOrder = @SortOrder, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE FilterGroupID = @FilterGroupID
END
