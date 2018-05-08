CREATE  PROC [dbo].[e_FilterCondition_UpdateSortOrder_FilterConditionID] 
(
	@UserID int = NULL,
    @FilterConditionID int = NULL,
    @SortOrder int = NULL
)
AS 
BEGIN
	Update FilterCondition SET SortOrder = @SortOrder, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE FilterConditionID = @FilterConditionID
END
