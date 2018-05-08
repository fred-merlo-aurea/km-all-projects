CREATE PROCEDURE [dbo].[e_LayoutPlans_Select_LayoutPlanID]   
@LayoutPlanID int = NULL
AS
	SELECT * FROM LayoutPlans WITH (NOLOCK) WHERE LayoutPlanID = @LayoutPlanID and IsDeleted = 0