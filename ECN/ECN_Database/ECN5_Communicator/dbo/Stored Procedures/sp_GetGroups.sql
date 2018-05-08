CREATE proc [dbo].[sp_GetGroups] 
(
	@ChannelID int,
	@CustomerID int,
	@userID int,
	@FolderID int,
	@CustomerHasUserDepartments char(1)
)
as
Begin
	Set nocount on
	declare @sqlString varchar(8000)

	if @CustomerHasUserDepartments = 'N' 
		SELECT g.GroupID, 
			(convert(varchar(255),g.GroupID) + '&chID=' + convert(varchar,@ChannelID) + '&cuID='  + convert(varchar,@CustomerID)) as GroupIDplus, 
			g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList
		FROM 
			Groups g left outer join 
			EmailGroups eg on eg.GroupID =g.GroupID AND eg.SubscribeTypeCode = 'S' 
		WHERE 
			g.CustomerID= @CustomerID AND 
			isnull(g.FolderID,0) = @FolderID  AND 
			g.GroupID in (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID))  
		GROUP BY g.GroupID, g.GroupName, g.IsSeedList ORDER BY g.GroupName 
	else        
		SELECT g.GroupID, 
			(convert(varchar(255),g.GroupID) + '&chID=' + convert(varchar,@ChannelID) + '&cuID='  + convert(varchar,@CustomerID)) as GroupIDplus, 
			g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList 
		FROM 
			Groups g left outer join 
			EmailGroups eg on eg.GroupID =g.GroupID AND eg.SubscribeTypeCode = 'S' JOIN
			DeptItemReferences dr on g.GroupID = dr.ItemID AND dr.Item = 'GRP'
		WHERE 
			g.CustomerID= @CustomerID AND 
			isnull(g.FolderID,0) = @FolderID  AND 
			g.GroupID in (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID))  AND
			dr.DepartmentID in (Select departmentID from [ECN5_ACCOUNTS].[DBO].UserDepartments where userID = @UserID)
		GROUP BY g.GroupID, g.GroupName, g.IsSeedList ORDER BY g.GroupName 
 
End
