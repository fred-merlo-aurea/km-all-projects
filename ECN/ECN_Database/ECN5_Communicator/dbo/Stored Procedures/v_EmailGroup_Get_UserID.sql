CREATE PROC [dbo].[v_EmailGroup_Get_UserID] 
(
	@CustomerID int,
	@UserID int
)

AS

SET NOCOUNT ON

SELECT 
	CASE WHEN ISNULL(f.folderID,'') = '' THEN 0 ELSE f.folderID END AS folderID, 
	CASE WHEN ISNULL(LTRIM(RTRIM(f.foldername)),'') = '' THEN 'Root' ELSE LTRIM(RTRIM(f.folderName)) END AS FolderName,
	LTRIM(RTRIM(g.GroupName)) AS GroupName, 
	COUNT(eg.EmailGroupID) AS Subscribers
FROM 
	[Groups] g WITH (NOLOCK)
	LEFT OUTER JOIN EmailGroups eg WITH (NOLOCK) ON g.groupID = eg.groupID 
	LEFT OUTER JOIN Folder f with (nolock) ON g.FolderID = f.folderID
WHERE 
	g.CustomerID = @CustomerID
	AND eg.SubscribeTypeCode = 'S'  
	AND f.IsDeleted = 0 
	AND g.GroupID in (SELECT GroupID FROM dbo.fn_getGroupsforUser(@CustomerID,@UserID))
GROUP BY 
	f.folderID, 
	f.foldername, 
	g.GroupName
ORDER BY 
	f.foldername, 
	g.GroupName
