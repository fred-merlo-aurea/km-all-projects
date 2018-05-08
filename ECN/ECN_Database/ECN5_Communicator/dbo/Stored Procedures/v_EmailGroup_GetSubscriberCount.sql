
CREATE proc [dbo].[v_EmailGroup_GetSubscriberCount] 
(
	@CustomerID int,
	@GroupID int
	
	--set @CustomerID = 1
	--set @GroupID = 49195
)
as
Begin
	Set nocount on
	
	SELECT COUNT(eg.EmailID)
	FROM 
		[EmailGroups] eg WITH (NOLOCK)
		JOIN [Groups] g WITH (NOLOCK) on eg.GroupID = g.GroupID
	WHERE 
		g.CustomerID= @CustomerID AND 
		eg.GroupID = @GroupID AND
		eg.SubscribeTypeCode = 'S'
End
