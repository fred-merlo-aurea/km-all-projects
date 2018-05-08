CREATE PROCEDURE [dbo].[MovedToActivity_sp_getGraphicalBlastBounceReportData] (
	@blastID int
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getGraphicalBlastBounceReportData', GETDATE())
	SELECT Distinct 
			substring(ActionNotes,0,50) as 'ActionNotes', 
			count(EmailID) as 'BounceCount', 
			ActionValue as 'BounceType'
	FROM EmailActivityLog  
	WHERE 
			BlastID = @blastID AND 
			ActionNotes is not null 
	group by 
			substring(ActionNotes,0,50), ActionValue 
	order by 
			ActionValue ASC, count(EmailID) DESC 
End
