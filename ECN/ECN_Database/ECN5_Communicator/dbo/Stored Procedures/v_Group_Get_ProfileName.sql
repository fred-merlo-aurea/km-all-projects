CREATE proc [dbo].[v_Group_Get_ProfileName] 
(
	@BaseChannelID int,
	@CustomerID int,
	@UserID int,
	@ProfileName varchar(200),
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
	--set @ProfileName = 'b'
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
declare @Where varchar(250)
SET @Where = ''
create table #emails (emailID int)
if @SearchCriteria = 'equals'
	Begin
		SET @Where = ' e.EmailAddress = ''' + @ProfileName + ''''
	End
	else if @SearchCriteria = 'starts'
	Begin
		SET @Where = ' e.EmailAddress like ''' + @ProfileName + '%'''
	End
	else if @SearchCriteria = 'like'
	Begin
		SET @Where = ' e.EmailAddress like ''%' + @ProfileName + '%'''
	End
	else if @SearchCriteria = 'ends'
	Begin
		SET @Where = ' e.EmailAddress like ''%' + @ProfileName + ''''
	End
EXEC('insert into #emails SELECT emailID from [Emails] e with (NOLOCK) WHERE e.CustomerID = ' + @CustomerID + ' AND ' + @Where);

WITH Results
	AS (SELECT ROW_NUMBER() OVER (ORDER BY
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
		join [EmailGroups] eg WITH (NOLOCK) on eg.GroupID = g.GroupID AND eg.SubscribeTypeCode in('S','U')
		join #emails e with(nolock) on eg.EmailID = e.EmailID
		join (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID)) tg on tg.GroupID = g.GroupID
	WHERE
		ISNULL(g.FolderID,0) = Case WHEN @FolderID is null THEN ISNULL(g.FolderID,0) ELSE @FolderID END
		AND ISNULL(g.Archived,0) = CASE WHEN @ArchiveFilter = 'archived' THEN 1 
										WHEN @ArchiveFilter = 'all' then ISNULL(g.Archived, 0)
										WHEN @ArchiveFilter = 'active' THEN 0 END 
		AND g.CustomerID = @CustomerID 
		AND IsNull(g.MasterSupression, 0) = 0 
        AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser( @CustomerID , @UserID))
	GROUP BY g.GroupID,f.FolderID, f.FolderName, g.GroupName, g.IsSeedList,ISNULL(g.Archived,0)
	HAVING COUNT(eg.EmailGroupID) <= case when @SubscriberLimit is not null then @SubscriberLimit
							when @SubscriberLimit is null then COUNT(eg.EmailGroupID) end
	)
	SELECT * 
	FROM Results
	WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
	drop table #emails

	--Set nocount on
	
	--declare @Where varchar(250)
	--SET @Where = ''
	
	--create table #emails (emailID int)
	
	----print @SelectSQL
	--if @SearchCriteria = 'equals'
	--Begin
	--	SET @Where = ' e.EmailAddress = ''' + @ProfileName + ''''
	--End
	--if @SearchCriteria = 'starts'
	--Begin
	--	SET @Where = ' e.EmailAddress like ''' + @ProfileName + '%'''
	--End
	--if @SearchCriteria = 'like'
	--Begin
	--	SET @Where = ' e.EmailAddress like ''%' + @ProfileName + '%'''
	--End
	--if @SearchCriteria = 'ends'
	--Begin
	--	SET @Where = ' e.EmailAddress like ''%' + @ProfileName + ''''
	--End

	
	--EXEC('insert into #emails SELECT emailID from [Emails] e with (NOLOCK) WHERE e.CustomerID = ' + @CustomerID + ' AND ' + @Where)			
	
	--SELECT	g.GroupID,f.FolderID, ISNULL(f.FolderName, '') as FolderName, convert(varchar(255),g.GroupID) + '&chID=' + convert(varchar,@BaseChannelID) + '&cuID=' + convert(varchar,@CustomerID) as GroupIDPlus, 
	--		g.GroupName, COUNT(eg.EmailGroupID) AS Subscribers, g.IsSeedList,ISNULL(g.Archived,0) as Archived
	--FROM 
	--		[Groups] g with (NOLOCK)
	--		left outer join [Folder] f with(nolock) on g.FolderID = f.FolderID
	--		join [EmailGroups] eg with (NOLOCK)  on eg.GroupID = g.GroupID 
	--		JOIN #emails te on te.emailID = eg.EmailID and eg.SubscribeTypeCode in('S','U')
	--		join (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID)) tg on tg.GroupID = g.GroupID 
	--WHERE IsNull(g.MasterSupression, 0) = 0 
	--AND ISNULL(f.FolderID,0) = case when @FolderID is null then ISNULL(f.FolderID,0) else @FolderID end
	--AND ISNULL(g.Archived, 0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(g.Archived, 0) when @ArchiveFilter = 'active' then 0 END
	--GROUP BY g.GroupID,f.FolderID, f.FolderName, g.GroupName, g.IsSeedList,g.Archived
	--ORDER BY g.GroupName 
	----print @SelectSQL
	
	--drop table #emails
	
 
End