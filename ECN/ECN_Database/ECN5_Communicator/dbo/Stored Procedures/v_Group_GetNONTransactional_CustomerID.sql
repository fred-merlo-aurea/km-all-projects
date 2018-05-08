CREATE proc [dbo].[v_Group_GetNONTransactional_CustomerID] 
(
	@CustomerID int
)
as
Begin
	Set nocount on
	 SELECT g.GroupID, g.GroupName 
	 FROM [Groups] g WITH (NOLOCK) JOIN GroupDataFields gdf WITH (NOLOCK) ON g.GroupID = gdf.GroupID  
	 WHERE g.CustomerID = @CustomerID AND DataFieldsetID IS Null GROUP BY g.GroupID, g.GroupName  Order by g.GroupName 
	
	
 
End