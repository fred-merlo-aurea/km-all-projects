CREATE  PROC [dbo].[e_BlastPlans_Delete_Single] 
(
	@CustomerID int,
    @BlastPlanID int,
    @UserID int
)
AS 
BEGIN
	Update BlastPlans set IsDeleted = 1, UpdatedUserID=@UserID, UpdatedDate=GetDate() WHERE BlastPlanID = @BlastPlanID AND CustomerID = @CustomerID
END