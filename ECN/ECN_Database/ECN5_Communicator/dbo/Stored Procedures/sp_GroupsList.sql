CREATE PROC [dbo].[sp_GroupsList] 
	@CustomerID varchar(25),
	@SelectedFolderID varchar(25),
	@chID_cuID varchar(255)
AS 
BEGIN 
	SELECT g.GroupID, (convert(varchar(255),g.GroupID) + @chID_cuID) as GroupIDplus, g.GroupName, 
	COUNT(eg.EmailGroupID) AS Subscribers 
	FROM Groups g left outer join [EmailGroups] eg on g.groupID = eg.groupID AND eg.SubscribeTypeCode = 'S' left outer join [Emails] e on e.emaiLID = eg.emailID
	WHERE g.CustomerID = @CustomerID and g.FolderID = @SelectedFolderID
	GROUP BY g.GroupID, g.GroupName  
	ORDER BY g.GroupName
END