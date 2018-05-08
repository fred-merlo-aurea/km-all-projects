CREATE PROCEDURE [dbo].[spGetEmailsinGroup]   
(  
	@groupID int,   
	@PageNo int,  
	@PageSize int,
	@searchString varchar(500)     
)       
as        
  
Begin      

	DECLARE @FirstRec int, @LastRec int
	
	if @PageNo = 1
	Begin
		set @FirstRec = 1;
		set @LastRec = @PageSize; 
	End
	Else
	Begin
		set @FirstRec = (@PageNo * @PageSize + 1);
		set @LastRec = (@FirstRec + @PageSize - 1); 
	End
	   
	CREATE TABLE #TempEmails (emailID int)		

	exec ('INSERT INTO #TempEmails SELECT e.EmailID from Emails e with (NOLOCK)  join EmailGroups eg with (NOLOCK)  ON e.EmailID = eg.EmailID WHERE eg.groupID = '+@groupID+' '+@searchString)	

	SELECT count(EmailID) from #TempEmails

	SET NOCOUNT ON;
	WITH TempResult (RowNum, EmailID, Emailaddress, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged) AS
	(
		SELECT  ROW_NUMBER() OVER(ORDER BY EmailAddress ASC) as 'RowNum',
				e.EmailID, 
				--'<div style="float:left">'+ Emailaddress+'</div><div style="float:right"><a style="text-decoration: none;" href="http://www.ecn5.com/ecn.profilemanager/engines/ecnProfileManager.aspx?ead='+ emailaddress+'&eid='+convert(varchar,e.EmailID)+'&cid='+convert(varchar,CustomerID)+'" target="_blank">&nbsp;</a></div>' AS EmailAddress,
				e.EmailAddress,
				eg.FormatTypeCode, 
				eg.SubscribeTypeCode, 
				eg.CreatedOn, 
				eg.LastChanged
		from	Emails e with (NOLOCK)  join 
				EmailGroups eg with (NOLOCK)  ON e.EmailID = eg.EmailID join
				#TempEmails te on te.emailID = e.emailID
	 	WHERE eg.groupID = @groupID 	
	)				
	SELECT	EmailID, Emailaddress, FormatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged, @groupID as GroupID
	FROM TempResult
	WHERE RowNum >= @FirstRec 
	AND RowNum <= @LastRec
	
	SET NOCOUNT OFF;

	drop table  #TempEmails
			
End

