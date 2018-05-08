CREATE PROCEDURE [dbo].[e_LayoutPlan_Exists] 
	@LayoutPlanID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 LayoutPlanID FROM LayoutPlans WITH (NOLOCK) WHERE CustomerID = @CustomerID AND LayoutPlanID = @LayoutPlanID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END