CREATE PROCEDURE [dbo].[v_EmailGroup_Get_Paging]   
(  
	@CustomerID INT,
	@GroupID INT,   
	@PageNo INT,  
	@PageSize INT,
	@SearchString VARCHAR(500)
)       
AS        
  
Begin  
	SET NOCOUNT ON;

	DECLARE @FirstRec INT, @LastRec INT
	
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
	
	CREATE TABLE #TempEmails (RowNum INT, EmailID INT, GroupID INT, EmailAddress VARCHAR(255), FormatTypeCode VARCHAR(5), SubscribeTypeCode VARCHAR(50), CreatedDate DATETIME, UpdatedDate DATETIME)	
	
	EXEC	('	INSERT INTO #TempEmails 
				SELECT 
						ROW_NUMBER() OVER(ORDER BY  isnull(eg.LastChanged, eg.CreatedOn) ASC), e.EmailID, ' + @GroupID + ', e.EmailAddress, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged 
				FROM 
					[Emails] e WITH (NOLOCK)  
					join [EmailGroups] eg WITH (NOLOCK)  ON e.EmailID = eg.EmailID 
				WHERE 
					e.CustomerID = ' + @CustomerID + ' AND 
					eg.groupID = ' + @GroupID + ' ' + @SearchString
			)	
	
	SELECT COUNT(EmailID) FROM #TempEmails
	
	SELECT 
		EmailID, GroupID, EmailAddress, FormatTypeCode, SubscribeTypeCode, CreatedDate, UpdatedDate 
	FROM 
		#TempEmails 
	WHERE 
		RowNum >= @FirstRec AND 
		RowNum <= @LastRec
	
	DROP table #TempEmails
	
	SET NOCOUNT OFF;


    
 --   declare @RecordNoStart int,  
	--    @RecordNoEnd int  
  
	--set nocount on
	
	--Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	--Set @RecordNoEnd = (@PageNo * @PageSize) + 50  
 	
	--if(LEN(@searchString) > 0)
	--BEGIN 

 --		SET ROWCOUNT @RecordNoEnd  

	--	CREATE TABLE #TempEmails (emailID int)		

	--	exec ('INSERT INTO #TempEmails SELECT e.EmailID from [Emails] e with (NOLOCK)  join [EmailGroups] eg with (NOLOCK)  ON e.EmailID = eg.EmailID WHERE e.CustomerID = '+@CustomerID + ' and eg.IsDeleted = 0 and eg.groupID = '+@GroupID+' '+@SearchString)	

	--	--exec ('SELECT count(e.EmailID) from [Emails] e with (NOLOCK) join [EmailGroups] eg with (NOLOCK)  ON e.EmailID = eg.EmailID  WHERE e.CustomerID = '+@CustomerID + '  and eg.IsDeleted = 0 and eg.groupID = '+@GroupID+' '+@SearchString)	

	--	SET ROWCOUNT 0 
		

	--	SELECT e.EmailID, @groupID as groupID, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedDate, eg.UpdatedDate, e.EmailAddress as EmailAddress
	--	from	[Emails] e with (NOLOCK)  join 
	--			[EmailGroups] eg with (NOLOCK)  ON e.EmailID = eg.EmailID join
	--			#TempEmails te on te.emailID = e.emailID
	-- 	WHERE eg.groupID = @GroupID and eg.IsDeleted = 0 and e.CustomerID = @CustomerID
	--	ORDER BY EmailAddress 

	--	DROP table #TempEmails
			  

	--END
	--ELSE
	--BEGIN

	--	exec('SELECT top ' + @RecordNoEnd + ' e.EmailID, ' + @groupID + ' AS GroupID, e.EmailAddress as EmailAddress,  eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedDate, eg.UpdatedDate ' + 
	--		' from [Emails] e with (NOLOCK) join [EmailGroups] eg with (NOLOCK)  ON e.EmailID = eg.EmailID ' +
	-- 		' WHERE eg.IsDeleted = 0 and eg.groupID = ' + @groupID + ' ORDER BY EmailAddress ')

	--END
	
End

