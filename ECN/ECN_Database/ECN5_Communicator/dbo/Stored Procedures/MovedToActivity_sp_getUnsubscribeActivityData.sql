CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getUnsubscribeActivityData]
(
	@CustomerID int,
	@Frequency  varchar(10)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getUnsubscribeActivityData', GETDATE())
	declare @sqlstring varchar(4000)

	if UPPER(@Frequency) = 'DAILY'
	begin
		set @sqlstring = 
				'SELECT e.EmailAddress, ''U'' as ''ActionType'', eal.ActionDate '+
				' FROM Emailactivitylog eal '+
				'JOIN Emails e ON eal.emailID = e.EmailID '+
				'WHERE e.CustomerID = 1214 '+
				'AND (eal.ActionTypeCode=''subscribe'' OR eal.ActionTypeCode=''ABUSERPT_UNSUB'' OR eal.ActionTypeCode=''FEEDBACK_UNSUB'') '+
				'AND CONVERT(VARCHAR,eal.ActionDate,101) = CONVERT(VARCHAR, getDate()-1,101) '+
				'ORDER BY eal.ActionDate'
		exec (@sqlstring)
	
	end
End
