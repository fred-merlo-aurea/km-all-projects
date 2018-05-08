CREATE  PROCEDURE [dbo].[sp_getBlastEmailActivityData] (
	@CustomerID varchar(10),
	@Actioncode varchar(100)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBlastEmailActivityData', GETDATE())
	declare @sqlstring varchar(8000)

	set @sqlstring = ' SELECT eal.BlastID, eal.EmailID, eal.ActionTypeCode, eal.ActionDate, eal.ActionValue,'+ 
			' e.EmailAddress, e.Title, e.FirstName, e.LastName, e.FullName, e.Company, e.Occupation, e.Address, e.Address2, e.City, e.State, e.Zip, e.Country,'+
			' e.Voice, e.Mobile, e.Fax, e.Website, e.Age, e.Income, e.Gender, e.User1, e.User2, e.User3, e.User4, e.User5, e.User6, '+
			' e.Birthdate, e.UserEvent1, e.UserEvent1Date, e.UserEvent2, e.UserEvent2Date'+
			' FROM Emailactivitylog eal '+
			' JOIN Emails e ON eal.emailID = e.EmailID'+
			' JOIN [BLAST] b on eal.BlastID = b.blastID'+
			' WHERE b.CustomerID = '+@CustomerID+' and e.CustomerID = '+@CustomerID+
			' AND eal.ActionTypeCode IN ('+@Actioncode+') '+
			' AND CONVERT(VARCHAR,eal.ActionDate,101) = CONVERT(VARCHAR, getDate()-1,101) '+
			' ORDER BY eal.BlastID, eal.ActionDate ';
	exec (@sqlstring)
End
