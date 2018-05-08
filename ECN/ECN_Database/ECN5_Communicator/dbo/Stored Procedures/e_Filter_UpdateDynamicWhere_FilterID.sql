CREATE  PROC [dbo].[e_Filter_UpdateDynamicWhere_FilterID] 
(
	@UserID int = NULL,
    @FilterID int = NULL,
    @DynamicWhere text = NULL
)
AS 
BEGIN
	Update Filter SET DynamicWhere = @DynamicWhere, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE FilterID = @FilterID
END
