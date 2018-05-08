CREATE PROCEDURE [job_UpdateEmailStatus]
@Xml xml,
@status varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @docHandle int,
			@EmailStatusID int,
			@UnsubscribeEmailStatusID int

	select @EmailStatusID = EmailStatusID 
	from EmailStatus 
	where Status = @status
	
	select @UnsubscribeEmailStatusID = EmailStatusID 
	from EmailStatus 
	where Status = 'UnSubscribe'
	

	CREATE TABLE #import 
		(  
			PubSubscriptionID int,  UpdateReason varchar(200), UpdateDate datetime
		) 
	CREATE INDEX EA_1 on #import (PubSubscriptionID)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

	Insert Into #import (PubSubscriptionID,UpdateReason,UpdateDate)
	select ps.PubSubscriptionID,  x.UpdateReason, isnull(x.UpdateDate, GETDATE())
	from
		(
			Select EmailAddress,UpdateReason,UpdateDate
			FROM OPENXML(@docHandle, N'/XML/Emails')
			WITH   
				(  
					EmailAddress varchar(100) 'EmailAddress', UpdateReason varchar(200) 'UpdateReason', UpdateDate datetime 'UpdateDate'
				) 
		) x 
		join PubSubscriptions ps on x.emailaddress = ps.EMAIL
	where rtrim(ltrim(isnull(x.Emailaddress,''))) <> '' 
		
	EXEC sp_xml_removedocument @docHandle    

	Update ps
	set	EmailStatusID = @EmailStatusID,
		StatusUpdatedReason = i.UpdateReason,
		StatusUpdatedDate = i.UpdateDate
	from #import i 
		join PubSubscriptions ps on ps.PubSubscriptionID = i.PubSubscriptionID
	where ps.EmailStatusID <> @UnsubscribeEmailStatusID

End