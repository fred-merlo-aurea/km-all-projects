CREATE proc [dbo].[v_Group_GetGroupDR] 
(
	@CustomerID int,
	@UserID int
)
as
Begin
	Set nocount on
	SELECT GroupName, GroupID
	FROM 
		[Groups] WITH (NOLOCK)
	WHERE 
		CustomerID= @CustomerID AND 
		GroupID in (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID))  
	ORDER BY GroupName 

 
End