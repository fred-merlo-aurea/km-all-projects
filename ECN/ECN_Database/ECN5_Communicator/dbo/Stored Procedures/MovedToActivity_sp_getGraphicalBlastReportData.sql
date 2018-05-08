CREATE proc [dbo].[MovedToActivity_sp_getGraphicalBlastReportData] (
		@blastID int
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getGraphicalBlastReportData', GETDATE())
	SELECT  EmailID, ActionTypeCode, ActionDate,  
			convert(varchar, ActionDate, 101) as 'ActionDateMMDDYYYY', ActionValue, ActionNotes   
	FROM 
			EmailActivityLog   
	WHERE 
			BlastID =  @blastID AND 
			(  
				 Actiontypecode LIKE 'send' OR Actiontypecode LIKE 'bounce' OR Actiontypecode LIKE 'click' OR  
				 Actiontypecode LIKE 'open' OR Actiontypecode LIKE 'conv%' OR Actiontypecode LIKE 'subscribe' OR  
				 Actiontypecode LIKE 'resend' OR Actiontypecode LIKE 'refer' OR Actiontypecode LIKE 'testsend' 
			) 
End
