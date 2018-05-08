CREATE proc [dbo].[v_LayoutPlans_GetCampaignLayoutPlanSummary] 
(
	@CustomerID int
)
as
Begin
	Set nocount on
	SELECT l.LayoutID, l.LayoutName, COUNT(l.LayoutID) AS TriggerCount
	FROM 
		LayoutPlans lp WITH (NOLOCK)
		JOIN Layout l WITH (NOLOCK) ON lp.LayoutID = l.LayoutID and l.IsDeleted = 0
	WHERE 
		lp.CustomerID= @CustomerID AND
		lp.IsDeleted = 0 and lp.EventType not in('submit','abandon')
	GROUP BY l.LayoutID, l.LayoutName ORDER BY l.LayoutName

 
End