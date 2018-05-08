CREATE PROCEDURE [dbo].[sp_getEmailsListFromGroup_paging]   
(  
	@groupID int,   
	@PageNo int,  
	@PageSize int,
	@searchString varchar(500)
)       
as        
  
Begin      
    declare @RecordNoStart int,  
	    @RecordNoEnd int  
  
	set nocount on
	
	Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	Set @RecordNoEnd = (@PageNo * @PageSize) + 50  
 	
	if(LEN(@searchString) > 0)
	BEGIN 

 		SET ROWCOUNT @RecordNoEnd  

		CREATE TABLE #TempEmails (emailID int)		

		exec ('INSERT INTO #TempEmails SELECT e.EmailID from Emails e with (NOLOCK)  join EmailGroups eg with (NOLOCK)  ON e.EmailID = eg.EmailID WHERE eg.groupID = '+@groupID+' '+@searchString)	

		exec ('SELECT count(e.EmailID) from Emails e with (NOLOCK) join EmailGroups eg with (NOLOCK)  ON e.EmailID = eg.EmailID  WHERE eg.groupID = '+@groupID+' '+@searchString)	

		SET ROWCOUNT 0 
		

		SELECT e.EmailID, @groupID as groupID, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged,
				'<div style="float:left">'+e.emailaddress+'</div><div style="float:right"><a style="text-decoration: none;" href="http://www.ecn5.com/ecn.profilemanager/engines/ecnProfileManager.aspx?ead='+e.emailaddress+'&eid='+convert(varchar,e.emailID)+'&cid='+convert(varchar,e.CustomerID)+'" target="_blank">&nbsp;</a></div>' AS EmailAddress
		from	Emails e with (NOLOCK)  join 
				EmailGroups eg with (NOLOCK)  ON e.EmailID = eg.EmailID join
				#TempEmails te on te.emailID = e.emailID
	 	WHERE eg.groupID = @groupID 
		ORDER BY EmailAddress 

		DROP table #TempEmails
			  

	END
	ELSE
	BEGIN
		SELECT COUNT(EmailGroupID) FROM EmailGroups WHERE GroupID = @groupID		

		/*exec('SELECT top ' + @RecordNoEnd + ' e.EmailID, ' + @groupID + ' AS GroupID, e.EmailAddress, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged ' + 
			' from Emails e join EmailGroups eg ON e.EmailID = eg.EmailID ' +
	 		' WHERE eg.groupID = ' + @groupID + ' ORDER BY EmailAddress ')*/

		-- Commented the above to drop a temp link to the Profile Manager. - ashok 05/08/08
		exec('SELECT top ' + @RecordNoEnd + ' e.EmailID, ' + @groupID + ' AS GroupID, ''<div style="float:left">''+e.emailaddress+''</div><div style="float:right"><a style="text-decoration: none;" href="http://www.ecn5.com/ecn.profilemanager/engines/ecnProfileM
anager.aspx?ead=''+e.emailaddress+''&eid=''+convert(varchar,e.emailID)+''&cid=''+convert(varchar,e.CustomerID)+''" target="_blank">&nbsp;</a></div>'' AS ''EmailAddress'',  eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged ' + 
			' from Emails e with (NOLOCK) join EmailGroups eg with (NOLOCK)  ON e.EmailID = eg.EmailID ' +
	 		' WHERE eg.groupID = ' + @groupID + ' ORDER BY EmailAddress ')

	END
	
End
