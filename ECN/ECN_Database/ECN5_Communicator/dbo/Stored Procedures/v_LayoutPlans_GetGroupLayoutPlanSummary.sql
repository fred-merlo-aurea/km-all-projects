CREATE proc [dbo].[v_LayoutPlans_GetGroupLayoutPlanSummary] 
(
	@CustomerID int
)
as
Begin
	Set nocount on
	SELECT g.GroupID, g.GroupName, COUNT(g.GroupID) AS TriggerCount
	FROM 
		LayoutPlans lp WITH (NOLOCK)
		JOIN [Groups] g WITH (NOLOCK) ON lp.GroupID = g.GroupID
	WHERE 
		lp.CustomerID= @CustomerID AND
		lp.IsDeleted = 0
	GROUP BY g.GroupID, g.GroupName ORDER BY g.GroupName

 
End