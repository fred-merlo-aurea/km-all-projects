CREATE  PROC [dbo].[e_LayoutPlans_Delete_All] 
(
	@UserID int = NULL,
    @LayoutPlanID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update LayoutPlans SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE LayoutPlanID = @LayoutPlanID AND CustomerID = @CustomerID
END
