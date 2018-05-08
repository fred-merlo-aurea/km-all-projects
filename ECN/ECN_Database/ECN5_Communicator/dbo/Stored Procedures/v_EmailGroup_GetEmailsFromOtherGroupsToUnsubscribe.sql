CREATE PROCEDURE [dbo].[v_EmailGroup_GetEmailsFromOtherGroupsToUnsubscribe]  
(
	@CustomerID int,
	@FolderID int,
	@EmailIDs varchar(400)
) AS 
BEGIN
	SELECT 
		DISTINCT e.EmailID 
	FROM 
		[Emails] e WITH (NOLOCK)
		JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
		JOIN [Groups] g WITH (NOLOCK) on eg.GroupID = g.groupID 
	WHERE 
		IsNull(g.FolderID, 0) = @FolderID AND 
		g.CustomerID = @CustomerID AND
		eg.SubscribeTypeCode <> 'U' AND 
		eg.EmailID IN (@EmailIDs)
END