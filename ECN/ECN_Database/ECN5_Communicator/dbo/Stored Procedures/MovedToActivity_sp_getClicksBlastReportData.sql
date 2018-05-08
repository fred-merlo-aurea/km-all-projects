CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getClicksBlastReportData] (
	@blastID varchar(10),
	@clicksReportType varchar(15)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getClicksBlastReportData', GETDATE())
	declare @sqlstring varchar (4000)

	if lower(@clicksReportType) = 'topclicks'
	Begin
		set @sqlstring=	' SELECT TOP 10 Count(eal.ActionValue) AS ClickCount, eal.EmailID, e.EmailAddress, '+
				' ''EmailID=''+CONVERT(VARCHAR,eal.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'' '+
				' FROM Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID JOIN [BLAST] b on eal.BlastID = b.BlastID '+
				' WHERE eal.ActionTypeCode=''click'' '+
				' AND eal.BlastID='+@blastID+
				' GROUP BY e.EmailAddress, eal.EmailID,''EmailID=''+CONVERT(VARCHAR,eal.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID)'+
				' ORDER BY ClickCount DESC, e.EmailAddress ';
 	end 
 	else if lower(@clicksReportType) = 'allclicks'
	Begin
		set @sqlstring=	' SELECT eal.EMailID as EmailID, e.EmailAddress as EmailAddress, eal.ActionDate as ClickTime, '+
				' eal.ActionValue AS FullLink, '+
				' ''EmailID=''+CONVERT(VARCHAR,eal.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'','+
				' CASE WHEN LEN(eal.ActionValue) > 6 THEN LEFT(RIGHT(eal.ActionValue,LEN(eal.ActionValue)-7),40) ELSE eal.ActionValue END AS SmallLink'+
				' FROM Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID JOIN [BLAST] b on eal.BlastID = b.BlastID '+
				' WHERE eal.BlastID='+@blastID+
				' AND ActionTypeCode=''click'' '+
				' ORDER BY ActionDate DESC';
	end

	exec (@sqlstring)
End
