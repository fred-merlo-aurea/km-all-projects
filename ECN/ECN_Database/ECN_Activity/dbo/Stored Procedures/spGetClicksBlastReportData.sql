CREATE  PROCEDURE [dbo].[spGetClicksBlastReportData] (
	@blastID varchar(10),
	@clicksReportType varchar(15)
)
as
Begin
	declare @sqlstring varchar (4000)

	if lower(@clicksReportType) = 'topclicks'
	Begin
		set @sqlstring=	' SELECT TOP 10 Count(URL) AS ClickCount, bac.EmailID, e.EmailAddress, '+
				' ''EmailID=''+CONVERT(VARCHAR,bac.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'' '+
				' FROM ECN5_COMMUNICATOR..Emails e JOIN BlastActivityClicks bac on e.EMailID=bac.EMailID JOIN ECN5_COMMUNICATOR..[BLAST] b on bac.BlastID = b.BlastID '+
				' WHERE bac.BlastID='+@blastID+
				' GROUP BY e.EmailAddress, bac.EmailID,''EmailID=''+CONVERT(VARCHAR,bac.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID)'+
				' ORDER BY ClickCount DESC, e.EmailAddress ';
 	end 
 	else if lower(@clicksReportType) = 'allclicks'
	Begin
		set @sqlstring=	' SELECT bac.EMailID as EmailID, e.EmailAddress as EmailAddress, ClickTime as ClickTime, '+
				' URL AS FullLink, '+
				' ''EmailID=''+CONVERT(VARCHAR,bac.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'','+
				' CASE WHEN LEN(URL) > 6 THEN LEFT(RIGHT(URL,LEN(URL)-7),40) ELSE URL END AS SmallLink'+
				' FROM ECN5_COMMUNICATOR..Emails e JOIN BlastActivityClicks on e.EMailID=bac.EMailID JOIN [BLAST] b on bac.BlastID = b.BlastID '+
				' WHERE bac.BlastID='+@blastID+
				' ORDER BY ClickTime DESC';
	end

	exec (@sqlstring)
End
