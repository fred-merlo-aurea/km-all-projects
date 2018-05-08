CREATE PROCEDURE [dbo].[v_EmailGroup_Get_Paging_By_Date]
(
	@CustomerID INT,
	@GroupID INT,   
	@PageNo INT,  
	@PageSize INT,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@Recent BIT,
	@Filter VARCHAR(500),
	@SortColumn varchar(50) = 'EmailID',
	@SortDirection varchar(50) = 'ASC'
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE 
		@FirstRec	INT, 
		@LastRec	INT,
		@SelectSQL	VARCHAR(MAX),
		@RecentSQL	VARCHAR(MAX),
		@FinalSQL	VARCHAR(MAX),
		@OrderSQL	VARCHAR(MAX),
		@FromDate_C VARCHAR(30),
		@ToDate_C	VARCHAR(30)
	
	IF @PageNo = 1
	BEGIN
		SET @FirstRec = 1;
		SET @LastRec = @PageSize; 
	END
	ELSE
	BEGIN
		SET @FirstRec = ((@PageNo - 1) * @PageSize + 1);
		SET @LastRec = (@FirstRec + @PageSize - 1); 
	END

	SET @FromDate_C = CONVERT(VARCHAR(23),@FromDate ,121)
	SET @ToDate_C   = CONVERT(VARCHAR(23),@ToDate + '23:59:59' ,121)  
	

	CREATE TABLE #TempEmails (
		RowNum INT IDENTITY(1,1), 
		EmailID INT, 
		GroupID INT, 
		EmailAddress VARCHAR(255), 
		FormatTypeCode VARCHAR(5), 
		SubscribeTypeCode VARCHAR(1), 
		CreatedDate DATETIME, 
		UpdatedDate DATETIME,
		TotalCount int);	
	
	if @SortColumn = 'EmailID'
	BEGIN
		SET @OrderSQL = ' ORDER BY 
		e.EmailId ' + @SortDirection + ',
		eg.LastChanged, 
		eg.CreatedOn ASC '
	END
	else if @SortColumn = 'EmailAddress'
	BEGIN
		SET @OrderSQL = ' 
		ORDER BY 
			e.EmailAddress ' + @SortDirection + ',
			eg.LastChanged, 
			eg.CreatedOn ASC '
	END
	else if @SortColumn = 'FormatTypeCode'
	BEGIN
		SET @OrderSQL = ' 
		ORDER BY 
			eg.FormatTypeCode ' + @SortDirection + ',
			eg.LastChanged, 
			eg.CreatedOn ASC '
	END
	else if @SortColumn = 'SubscribeTypeCode' 
	BEGIN
		SET @OrderSQL = ' 
		ORDER BY 
			eg.SubscribeTypeCode ' + @SortDirection + ',
			eg.LastChanged, 
			eg.CreatedOn ASC '
	END
	else if @SortColumn = 'CreatedOn' or @SortColumn = 'DateAdded'
	BEGIN
		SET @OrderSQL = ' 
		ORDER BY 			
			eg.CreatedOn ' + @SortDirection 
			
	END
	else if @SortColumn = 'LastChanged' or @SortColumn = 'DateUpdated'
	BEGIN
		SET @OrderSQL = ' 
		ORDER BY 			
			eg.LastChanged ' + @SortDirection 
	END
	
	

    SET @SelectSQL = '
    INSERT INTO 
		#TempEmails 
	SELECT DISTINCT
		e.EmailID,
		' + CONVERT(VARCHAR(15), @GroupID) +' as GroupID, 
		e.EmailAddress, 
		eg.FormatTypeCode, 
		eg.SubscribeTypeCode, 
		eg.CreatedOn, 
		eg.LastChanged  ,

		COUNT(e.EmailID)
		
    FROM 
		Emails e WITH (NOLOCK)  
		INNER JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
	WHERE 
		e.CustomerID = ' + CONVERT(VARCHAR(10),@CustomerID) + '
		AND eg.groupID = ' + CONVERT(VARCHAR(10),@GroupID) 
		+ ' ' + @Filter + '	
		AND (eg.CreatedOn BETWEEN ''' + @FromDate_C + ''' AND ''' + @ToDate_C + 
		''' or eg.LastChanged BETWEEN  ''' + @FromDate_C + ''' AND ''' + @ToDate_C + ''')
		GROUP BY e.EMailID, GroupID, e.EmailAddress, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged'

    SET @RecentSQL = '
    INSERT INTO 
		#TempEmails 
	SELECT DISTINCT
		e.EmailID,
		' + CONVERT(VARCHAR(15), @GroupID) +' as GroupID, 
		e.EmailAddress, 
		eg.FormatTypeCode, 
		eg.SubscribeTypeCode, 
		eg.CreatedOn, 
		eg.LastChanged  ,

		COUNT(e.EmailID)
    FROM 
		Emails e WITH (NOLOCK)  
		INNER JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
		INNER JOIN ECN_Activity..BlastActivityOpens bao WITH (NOLOCK) ON bao.EmailID = eg.EmailID 
--		LEFT OUTER JOIN ECN_Activity..BlastActivityClicks bac WITH (NOLOCK) ON bac.EmailID = eg.EmailID
	WHERE 
		e.CustomerID = ' + CONVERT(VARCHAR(10),@CustomerID) + '
		AND eg.groupID = ' + CONVERT(VARCHAR(10),@GroupID) 
		+ ' ' + @Filter + '	
		AND (eg.CreatedOn BETWEEN ''' + @FromDate_C + ''' AND ''' + @ToDate_C + ''' or eg.LastChanged BETWEEN  ''' + @FromDate_C + ''' AND ''' + @ToDate_C + ''')
		AND (bao.OpenTime BETWEEN ''' + @FromDate_C + ''' AND ''' + @ToDate_C + ''')
	GROUP BY e.EMailID, GroupID, e.EmailAddress, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged '
			
	
	IF @Recent = 1
	BEGIN
		--PRINT ( @RecentSQL + @OrderSQL)	
		EXEC ( @RecentSQL + @OrderSQL)

	END
	ELSE
	BEGIN
		--PRINT (@SelectSQL + @OrderSQL)	
		EXEC (@SelectSQL + @OrderSQL)
	END
	
	declare @TotalCount int
	SELECT COUNT(EmailID) FROM #TempEmails
	Select @TotalCount = COUNT(EmailID) from #TempEmails
	
	
	SELECT 
		EmailID, 
		GroupID, 
		EmailAddress, 
		FormatTypeCode, 
		SubscribeTypeCode, 
		CreatedDate, 
		UpdatedDate ,
		CreatedDate as DateAdded,
		UpdatedDate as DateUpdated,
		@TotalCount as TotalCount
	FROM 
		#TempEmails 
	WHERE 
		RowNum >= @FirstRec 
		AND RowNum <= @LastRec
	
	DROP TABLE #TempEmails

	SET NOCOUNT OFF;
END
