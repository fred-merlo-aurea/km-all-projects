CREATE  PROC [dbo].[e_TriggerPlans_Delete] 
(
	@UserID int = NULL,
    @TriggerPlanID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update TriggerPlans SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE TriggerPlanID = @TriggerPlanID AND CustomerID = @CustomerID
END
