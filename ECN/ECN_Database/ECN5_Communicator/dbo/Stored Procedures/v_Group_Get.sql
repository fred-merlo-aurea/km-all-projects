CREATE proc [dbo].[v_Group_Get] 
(
	@BaseChannelID int,
	@CustomerID int,
	@UserID int,
	@FolderID int = null,
	@ArchiveFilter varchar(20) = 'all',
	@CurrentPage int = 1,
	@PageSize int = 15,
	@SubscriberLimit int = null
)
as
Begin
	WITH Results
	AS (SELECT ROW_NUMBER() OVER (ORDER BY GroupName) AS ROWNUM,Count(*) over () AS TotalCount,
	 g.GroupID, f.FolderID, ISNULL(f.FolderName,'') as FolderName,
		(convert(varchar(255),g.GroupID) + '&chID=' + convert(varchar,@BaseChannelID) + '&cuID='  + convert(varchar,@CustomerID)) as GroupIDPlus, 
		g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList as IsSeedList,ISNULL(g.Archived,0) as Archived
	FROM 
		[Groups] g WITH (NOLOCK) left outer join 
		[EmailGroups] eg WITH (NOLOCK) on eg.GroupID =g.GroupID AND eg.SubscribeTypeCode = 'S' 
		left join Folder f (NOLOCK) on f.FolderID = g.FolderID
	WHERE 
		g.CustomerID= @CustomerID AND 
		ISNULL(g.MasterSupression, 0) = 0 AND
		isnull(g.FolderID,0) = CASE WHEN @FolderID is null then isnull(g.FolderID,0) else @FolderID end AND 
		g.GroupID in (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID))  
		and ISNULL(g.Archived,0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(g.Archived,0) when @ArchiveFilter = 'active' then 0 END
	GROUP BY g.GroupID, g.GroupName, g.IsSeedList ,f.FolderID, f.FolderName,ISNULL(g.Archived,0)
	HAVING COUNT(eg.EmailGroupID) <= case when @SubscriberLimit is not null then @SubscriberLimit
							when @SubscriberLimit is null then COUNT(eg.EmailGroupID) end
	)
	SELECT * From Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)

 
End