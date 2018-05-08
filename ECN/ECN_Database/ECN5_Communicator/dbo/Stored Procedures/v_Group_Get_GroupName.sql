CREATE proc [dbo].[v_Group_Get_GroupName] 
(
	@BaseChannelID int,
	@CustomerID int,
	@UserID int,
	@GroupName varchar(200),
	@SearchCriteria varchar(10),
	@FolderID int = null,
	@ArchiveFilter varchar(20) = 'all',
	@CurrentPage int = 1,
	@PageSize int = 15,
	@SubscriberLimit int = null,
	@SortColumn varchar(50) = 'GroupName',
	@SortDirection varchar(10) = 'ASC'
	
	--set @BaseChannelID = 12
	--set @CustomerID = 1
	--set @UserID = 4496
	--set @FolderID = 4091
	--set @CustomerHasUserDepartments = 'N'
	--set @GroupName = 'bill'
	--set @SearchCriteria = 'like'
	
--SELECT g.GroupID, (convert(varchar(255),g.GroupID) + '&chID=' + convert(varchar,@BaseChannelID) + '&cuID=' + convert(varchar,@CustomerID)) as GroupIDPlus, 
--						g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList
--						FROM [Groups] g left outer join 
--						[EmailGroups] eg on eg.GroupID = g.GroupID AND eg.SubscribeTypeCode = 'S'  WHERE g.CustomerID = @CustomerID 
--                    AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser( convert(varchar,@CustomerID) , convert(varchar,@UserID) ))  AND g.GroupName like '%bill%' GROUP BY g.GroupID, g.GroupName, g.IsSeedList 
--					ORDER BY g.GroupName 
	
	
)
as
Begin
	WITH Results
	AS (SELECT ROW_NUMBER() OVER(ORDER BY
		CASE WHEN (@SortColumn = 'GroupName' AND @SortDirection='ASC') THEN GroupName END ASC,
		CASE WHEN (@SortColumn = 'GroupName' AND @SortDirection='DESC') THEN GroupName END DESC,
		CASE WHEN (@SortColumn = 'IsSeedList' AND @SortDirection='ASC') THEN IsSeedList END ASC,
		CASE WHEN (@SortColumn = 'IsSeedList' AND @SortDirection='DESC') THEN IsSeedList END DESC,
		CASE WHEN (@SortColumn = 'Subscribers' AND @SortDirection = 'ASC') then COUNT(eg.EmailGroupID) END ASC,
		CASE WHEN (@SortColumn = 'Subscribers' AND @SortDirection = 'DESC') then COUNT(eg.EmailGroupID) END DESC	
    ) AS ROWNUM,Count(*) over () AS TotalCount,
		g.GroupID, f.FolderID, ISNULL(f.FolderName,'') as FolderName,(convert(varchar(255),g.GroupID) + '&chID=' + '' + convert(varchar,@BaseChannelID) + '' + '&cuID=' + '' + convert(varchar,@CustomerID) + '') as GroupIDPlus, 
		g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList,ISNULL(g.Archived,0) as Archived
	FROM [Groups] g WITH (NOLOCK)
		left outer join [Folder] f with(nolock) on g.FolderID = f.FolderID
		left outer join [EmailGroups] eg WITH (NOLOCK) on eg.GroupID = g.GroupID AND eg.SubscribeTypeCode = 'S'
	WHERE
		ISNULL(g.FolderID,0) = Case WHEN @FolderID is null THEN ISNULL(g.FolderID,0) ELSE @FolderID END
		AND ISNULL(g.Archived,0) = CASE WHEN @ArchiveFilter = 'archived' THEN 1 
										WHEN @ArchiveFilter = 'all' then ISNULL(g.Archived, 0)
										WHEN @ArchiveFilter = 'active' THEN 0 END 
		AND 
		(
			(@SearchCriteria = 'equals' AND g.GroupName = @GroupName)
			OR 
			(@SearchCriteria = 'starts' AND g.GroupName like @GroupName + '%')
			OR
			(@SearchCriteria = 'like' AND g.GroupName like '%' + @GroupName + '%')
			OR
			(@SearchCriteria = 'ends' AND g.GroupName like '%' + @GroupName)
		)
		AND g.CustomerID = @CustomerID AND IsNull(g.MasterSupression, 0) = 0 
        AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser( @CustomerID , @UserID))
	GROUP BY g.GroupID,f.FolderID, f.FolderName, g.GroupName, g.IsSeedList,ISNULL(g.Archived,0)
	HAVING COUNT(eg.EmailGroupID) <= case when @SubscriberLimit is not null then @SubscriberLimit
							when @SubscriberLimit is null then COUNT(eg.EmailGroupID) end
	)
	SELECT * 
	FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
	--declare @SelectSQL varchar(8000)
	--declare @Where varchar(250)
	--SET @Where = ''
	
	--SET @SelectSQL = 'SELECT g.GroupID, f.FolderID, ISNULL(f.FolderName,'''') as FolderName,(convert(varchar(255),g.GroupID) + ''&chID='' + ''' + convert(varchar,@BaseChannelID) + ''' + ''&cuID='' + ''' + convert(varchar,@CustomerID) + ''') as GroupIDPlus, 
	--					g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList,ISNULL(g.Archived,0) as Archived
	--					FROM [Groups] g WITH (NOLOCK)
	--					left outer join [Folder] f with(nolock) on g.FolderID = f.FolderID
	--					 left outer join [EmailGroups] eg WITH (NOLOCK) on eg.GroupID = g.GroupID AND eg.SubscribeTypeCode = ''S'' '

	--print @SelectSQL
	
	----print @SelectSQL
	--if @SearchCriteria = 'equals'
	--Begin
	--	SET @Where = ' AND g.GroupName = ''' + @GroupName + ''''
	--End
	--if @SearchCriteria = 'starts'
	--Begin
	--	SET @Where = ' AND g.GroupName like ''' + @GroupName + '%'''
	--End
	--if @SearchCriteria = 'like'
	--Begin
	--	SET @Where = ' AND g.GroupName like ''%' + @GroupName + '%'''
	--End
	--if @SearchCriteria = 'ends'
	--Begin
	--	SET @Where = ' AND g.GroupName like ''%' + @GroupName + ''''
	--End
	--if @FolderID is not null
	--BEGIN
	--	SET @Where += ' AND ISNULL(f.FolderID,0) = ' + Convert(varchar(20),@FolderID)
	--END
	
	--if(@ArchiveFilter = 'archived')
	--BEGIN
	--	SET @Where += ' AND ISNULL(g.Archived,0) = 1 '
	--END
	--ELSE IF(@ArchiveFilter = 'active')
	--BEGIN
	--	SET @Where += ' AND ISNULL(g.Archived,0) = 0 '
	--END

	----print @SelectSQL
	--SET @SelectSQL = @SelectSQL + ' WHERE g.CustomerID = ' + convert(varchar,@CustomerID) + ' AND IsNull(g.MasterSupression, 0) = 0 
 --                   AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser( ' + convert(varchar,@CustomerID) + ' , ' + convert(varchar,@UserID) + ' )) ' +
	--				@Where +
 --                   ' GROUP BY g.GroupID,f.FolderID, f.FolderName, g.GroupName, g.IsSeedList,ISNULL(g.Archived,0)
	--				ORDER BY g.GroupName '
	----print @SelectSQL
	--EXEC(@SelectSQL)
	
	
 
End