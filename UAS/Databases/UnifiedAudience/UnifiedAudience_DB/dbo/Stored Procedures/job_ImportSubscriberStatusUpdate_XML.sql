Create PROCEDURE [dbo].[job_ImportSubscriberStatusUpdate_XML]
@Xml xml
AS
BEGIN

	SET NOCOUNT ON

    DECLARE @docHandle int,
                    @UnsubscribeEmailStatusID int
                     
    select @UnsubscribeEmailStatusID = EmailStatusID from EmailStatus where Status = 'UnSubscribe'

    CREATE TABLE #import 
            (  
                    PubSubscriptionID int, EmailStatusID int
            ) 
    CREATE INDEX EA_1 on #import (PubSubscriptionID)

    EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

    Insert Into #import (PubSubscriptionID , EmailStatusID)
            select ps.PubSubscriptionID, x.EmailStatusID
            from
            (
                    SELECT 
                        EmailAddress, EmailStatusID
                    FROM OPENXML(@docHandle, N'/XML/StatusUpdate')   
                    WITH   
                    (  
                        EmailAddress varchar(100) 'EmailAddress', EmailStatusID int 'EmailStatusID'
                    )  
            ) x 
				join PubSubscriptions ps with (NOLOCK) on x.emailaddress = ps.EMAIL
            where rtrim(ltrim(isnull(x.Emailaddress,''))) <> '' and 
				x.emailstatusID in (select emailstatusID from emailstatus where status in ('Bounced','MasterSuppressed','Spam'))

    EXEC sp_xml_removedocument @docHandle   
       
    Update ps
    set EmailStatusID = i.EmailStatusID,
		StatusUpdatedReason = 'Update via file import',
		StatusUpdatedDate = getdate()
    from #import i 
		join PubSubscriptions ps on ps.PubSubscriptionID = i.PubSubscriptionID
    where ps.EmailStatusID <> @UnsubscribeEmailStatusID
              
    DROP TABLE #import
       
End
go