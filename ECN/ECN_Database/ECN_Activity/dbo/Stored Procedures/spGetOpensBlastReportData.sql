CREATE PROCEDURE [dbo].[spGetOpensBlastReportData] (
	@blastID varchar(10),
	@opensReportType varchar(15)
)
as
Begin
	declare @sqlstring varchar (4000)

	if lower(@opensReportType) = 'activeopens'
	Begin
		set @sqlstring=	' SELECT TOP 15 '+
				' COUNT(baop.emailID) AS ActionCount, baop.emailID, e.emailaddress, '+
				' ''EmailID=''+CONVERT(VARCHAR,baop.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'' '+
				' FROM  ecn5_communicator..Emails e JOIN BlastActivityOpens baop ON baop.emailid=e.emailid JOIN ecn5_communicator..[BLAST] b ON baop.BlastID = b.BlastID '+
				' WHERE baop.blastid='+@blastID+ 				
				' group by baop.emailid, e.emailaddress, ''EmailID=''+CONVERT(VARCHAR,baop.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) '+
				' order by ActionCount desc ';
 	end 
 	else if lower(@opensReportType) = 'allopens'
	Begin
		set @sqlstring=	' SELECT baop.EMailID, e.EmailAddress, baop.OpenTime as ActionDate, '' as ActionValue, b.GroupID, '+
				' ''EmailID=''+CONVERT(VARCHAR,baop.EmailID)+''&GroupID=''+CONVERT(VARCHAR,b.GroupID) AS ''URL'' '+
				' FROM ecn5_communicator..Emails e JOIN BlastActivityOpens baop ON e.EMailID=baop.EMailID JOIN ecn5_communicator..[BLAST] b ON baop.BlastID = b.BlastID  '+
				' WHERE baop.BlastID='+@blastID+ 				
				' ORDER BY ActionDate DESC';
	end

	exec (@sqlstring) 	
End
