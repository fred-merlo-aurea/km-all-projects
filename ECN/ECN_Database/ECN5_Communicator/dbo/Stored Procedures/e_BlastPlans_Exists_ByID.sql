CREATE PROCEDURE [dbo].[e_BlastPlans_Exists_ByID] 
	@BlastPlanID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 BlastPlanID FROM BlastPlans WHERE CustomerID = @CustomerID AND BlastPlanID = @BlastPlanID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
