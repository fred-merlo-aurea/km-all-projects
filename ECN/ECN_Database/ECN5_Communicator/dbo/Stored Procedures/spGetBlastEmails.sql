CREATE proc [dbo].[spGetBlastEmails] 
(    
	@BlastCat int = -1,				--send -1 if select is * for all
	@BlastType CHAR(1),    
	@UserID int,
	@CustomerID int,
	@HasUserDepts bit = 1,			--send 1 for true
	@SubjectSearch VARCHAR(50) = '',
	@GroupSearch VARCHAR(50) = '',
	@FromDate DateTime,
	@ToDate DateTime,
	@SentUserID int	= -1			--send -1 for all or actual selected sent user id
)    
as    
BEGIN  
	set NOCOUNT ON 
	DECLARE @SqlQuery varchar(8000)
	SET @SQLQuery = ''
	
	SELECT @SqlQuery = 'SELECT ' +
			'CASE ' +
				'WHEN blastgroupID IS NULL ' +
				'THEN blastID ' +
				'ELSE blastgroupID ' +
			'END AS ID, ' + 
			'CASE ' +
				'WHEN blastgroupID IS NULL ' +
				'THEN ''N'' ' +
				'ELSE ''Y'' ' +
			'END AS IsBlastGroup, ' +
			'MAX(CASE ' +
					'WHEN blastgroupID IS NULL ' +
					'THEN ''[''+CONVERT(VARCHAR(10),BlastID) +''] ''+ b.EmailSubject ' +
					'ELSE bg.emailsubject ' +
				'END ) AS emailsubject, ' + 
			'MAX(b.SendTime) AS sendtime, ' +
			'MAX(b.BlastType) AS blasttype, ' +
			'Sum(SendTotal) AS sendtotal, ' +  
			'MAX(CASE ' +
					'WHEN blastgroupID IS NULL ' +
					'THEN ((CASE ' +
								'WHEN b.FilterID = 2147483644 ' +
								'THEN ''Suppressed for BlastID ['' + Convert(varchar,b.refblastID) + '']'' ' +
								'WHEN b.FilterID = 2147483647 ' +
								'THEN ''UnClicked for BlastID ['' + Convert(varchar,b.refblastID) + '']'' ' +
								'WHEN b.FilterID = 2147483645 ' +
								'THEN ''UnOpened for BlastID ['' + Convert(varchar,b.refblastID) + '']'' ' +          
								'WHEN ISNULL(b.FilterID,0) <= 0 ' +
								'THEN ''< NO FILTER >'' ' +
								'WHEN f.FilterID <> 0 ' +
								'THEN f.FilterName ' +
								'ELSE ''< FILTER DELETED >'' ' +
							'END ) ) ' +
					'ELSE '''' ' + 
					'END ) AS Filtername, ' +    
					'MAX(CASE ' +
							'WHEN blastgroupID IS NULL ' +
							'THEN g.GroupName ' +
							'ELSE '''' ' +
						'END) AS groupname, ' +
			'MAX(b.TestBlast) as testblast, ' +
			'max(BlastID) AS bID ' +
		'FROM [BLAST] b ' +
		'LEFT OUTER JOIN [blastgrouping] bg ON (b.CreateduserID = bg.userID or IsNull(b.UpdatedUserID, 0) = bg.userID) and bg.blastIDs LIKE ''%'' + CONVERT(VARCHAR,b.blastID) + ''%'' '
	IF (@SentUserID > 0)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND bg.userID = ' + CONVERT(VARCHAR,@SentUserID)
	END
	SELECT @SqlQuery = @SqlQuery + ' LEFT OUTER JOIN [FILTER] f ON b.filterID = f.filterID LEFT OUTER JOIN [Groups] g ON b.GroupID = g.GroupID'
	IF (@HasUserDepts = 1)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' JOIN [DeptItemReferences] dr ON b.BlastID = dr.ItemID AND dr.Item = ''BLST'''
	END	
	SELECT @SqlQuery = @SqlQuery + ' WHERE b.CustomerID=' + CONVERT(VARCHAR,@CustomerID) +
		' AND (b.GroupID in (select GroupID from dbo.fn_getGroupsforUser(' + CONVERT(VARCHAR,@CustomerID) + ',' + CONVERT(VARCHAR,@UserID) +')) OR g.GroupID IS NULL)' + 
		' AND b.StatusCode=''sent'''
	IF (@BlastCat >= 0)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND CodeID = ' + CONVERT(VARCHAR,@BlastCat)
	END
	IF(@SentUserID > 0)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND (b.CreateduserID=' + CONVERT(VARCHAR,@SentUserID) + ' or IsNull(b.UpdatedUserID, 0)=' + CONVERT(VARCHAR,@SentUserID) + ') '
	END
	IF (@HasUserDepts = 1)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND dr.DepartmentID IN (SELECT DepartmentID FROM [ECN5_ACCOUNTS].[DBO].[UserDepartments] WHERE UserID = ' + CONVERT(VARCHAR,@UserID) + ')'
	END
	IF(@BlastType = 'Y' OR @BlastType = 'N')
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND TestBlast = ''' + @BlastType + ''''
	END	
	IF(LEN(@SubjectSearch) > 0)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND (b.EmailSubject like ''%' + @SubjectSearch + '%'' OR bg.EmailSubject like ''%' + @SubjectSearch + '%'')'
	END
	IF(LEN(@GroupSearch) > 0)
	BEGIN
		SELECT @SqlQuery = @SqlQuery + ' AND (GroupName like ''%' + @GroupSearch + '%'')'
	END
	SELECT @SqlQuery = @SqlQuery + ' AND (b.SendTime > ''' + CONVERT(VARCHAR,@FromDate) + ''' AND b.SendTime < ''' + CONVERT(VARCHAR,@ToDate) + ''')'
	SELECT @SqlQuery = @SqlQuery + ' GROUP BY ' +
				'CASE ' +
				'WHEN blastgroupID IS NULL ' +
				'THEN blastID ' +
				'ELSE blastgroupID END, ' +
			'CASE ' +
				'WHEN blastgroupID IS NULL THEN ''N'' ELSE ''Y'' ' +
			'END ' +
		'ORDER BY 4 DESC'	
		--print(@SqlQuery)
	Exec(@SqlQuery)
	
END
