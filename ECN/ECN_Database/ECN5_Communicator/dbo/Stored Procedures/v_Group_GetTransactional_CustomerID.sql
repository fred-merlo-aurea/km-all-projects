CREATE proc [dbo].[v_Group_GetTransactional_CustomerID] 
(
	@CustomerID int,
	@SearchWhere varchar(20),
	@SearchField varchar(20),
	@SearchCriteria varchar(50),
	@PageIndex int, 
	@PageSize int,
	@ArchiveFilter varchar(20),
	@AllFolders bit,
	@FolderID int = null
)
as
Begin
	Set nocount on;
	
	
	WITH Results
	AS (SELECT ROW_NUMBER() OVER (ORDER BY GroupName) AS ROWNUM,Count(*) over () AS TotalCount,
		g.GroupID, f.FolderID, ISNULL(f.FolderName,'') as FolderName, 
		g.GroupName
	FROM [Groups] g WITH (NOLOCK)
		join GroupDatafields gdf with(nolock) on g.GroupID = gdf.GroupID
		left outer join [Folder] f with(nolock) on g.FolderID = f.FolderID
		left outer join EmailGroups eg with(nolock) on g.groupid = eg.GroupID
		left outer join Emails e with(nolock) on eg.EmailID = e.EmailID and e.CustomerID = @CustomerID
	WHERE
		gdf.DatafieldSetID is not null 
		AND
		ISNULL(g.FolderID,0) = Case WHEN @FolderID is null or @AllFolders = 1 THEN ISNULL(g.FolderID,0) ELSE @FolderID END
		AND ISNULL(g.Archived,0) = CASE WHEN @ArchiveFilter = 'archived' THEN 1 
										WHEN @ArchiveFilter = 'all' then ISNULL(g.Archived, 0)
										WHEN @ArchiveFilter = 'active' THEN 0 END 
		AND 
		(
			(
			@SearchField = 'group' 
			AND
				(
					(@SearchWhere = 'equals' AND g.GroupName = @SearchCriteria)
					OR 
					(@SearchWhere = 'starts' AND g.GroupName like @SearchCriteria + '%')
					OR
					(@SearchWhere = 'like' AND g.GroupName like '%' + @SearchCriteria + '%')
					OR
					(@SearchWhere = 'ends' AND g.GroupName like '%' + @SearchCriteria)
				)
			)
			OR
			(
				@SearchField = 'profile'
				AND
				(
					(@SearchWhere = 'equals' AND e.emailaddress = @SearchCriteria)
					OR 
					(@SearchWhere = 'starts' AND e.emailaddress like @SearchCriteria + '%')
					OR
					(@SearchWhere = 'like' AND e.emailaddress like '%' + @SearchCriteria + '%')
					OR
					(@SearchWhere = 'ends' AND e.emailaddress like '%' + @SearchCriteria)
				)
			)

		)
		AND g.CustomerID = @CustomerID AND IsNull(g.MasterSupression, 0) = 0 
        
	GROUP BY g.GroupID,f.FolderID, f.FolderName, g.GroupName
	)
	SELECT * 
	FROM Results
	WHERE ROWNUM between ((@PageIndex - 1) * @PageSize + 1) and (@PageIndex * @PageSize)
 
End