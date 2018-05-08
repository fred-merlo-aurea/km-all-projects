CREATE PROCEDURE [dbo].[e_LayoutPlans_Delete_ByLPID]
	@LayoutPlanID int,
	@UserID int
AS
	if exists(Select top 1 * from LayoutPlans lp where lp.LayoutPlanID = @LayoutPlanID and ISNULL(lp.IsDeleted,0) = 0)
	BEGIN
		Update LayoutPlans
		set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
		where LayoutPlanID = @LayoutPlanID
	END
