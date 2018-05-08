CREATE  PROC [dbo].[e_LayoutPlans_Delete_Single] 
(
	@UserID int = NULL,
    @LayoutPlanID int = NULL,
    @LayoutID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update LayoutPlans SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE LayoutID = @LayoutID AND LayoutPlanID = @LayoutPlanID AND CustomerID = @CustomerID
END
