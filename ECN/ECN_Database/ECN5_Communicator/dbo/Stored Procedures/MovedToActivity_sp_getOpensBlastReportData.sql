CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getOpensBlastReportData] (
	@blastID varchar(10),
	@opensReportType varchar(15)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getOpensBlastReportData', GETDATE())
	declare @sqlstring varchar (4000)

	if lower(@opensReportType) = 'activeopens'
	Begin
		set @sqlstring=	' SELECT TOP 15 '+
				' COUNT(eal.emailID) AS ActionCount, eal.emailID, e.emailaddress, '+
				' ''EmailID=''+CONVERT(VARCHAR,eal.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'' '+
				' FROM  Emails e JOIN EmailActivityLog eal ON eal.emailid=e.emailid JOIN [BLAST] b ON eal.BlastID = b.BlastID '+
				' WHERE eal.blastid='+@blastID+
				' AND eal.ActionTypeCode=''open'' '+
				' group by eal.emailid, e.emailaddress, ''EmailID=''+CONVERT(VARCHAR,eal.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) '+
				' order by ActionCount desc ';
 	end 
 	else if lower(@opensReportType) = 'allopens'
	Begin
		set @sqlstring=	' SELECT eal.EMailID, e.EmailAddress, eal.ActionDate, eal.ActionValue, b.GroupID, '+
				' ''EmailID=''+CONVERT(VARCHAR,eal.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'' '+
				' FROM Emails e JOIN EmailActivityLog eal ON e.EMailID=eal.EMailID JOIN [BLAST] b ON eal.BlastID = b.BlastID  '+
				' WHERE eal.BlastID='+@blastID+
				' AND eal.ActionTypeCode=''open'' '+
				' ORDER BY ActionDate DESC';
	end

	exec (@sqlstring)
End
