
create procedure e_ImportSubscriberFile_UpdateMergedToUAD
@xml xml
as
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SystemSubscriberID int, SubscriptionID int, PublicationID int, account_id int
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SystemSubscriberID, SubscriptionID, PublicationID, account_id
	)  
	
	SELECT 
		SystemSubscriberID, SubscriptionID, PublicationID, account_id
	FROM OPENXML(@docHandle, N'/XML/ImportSubscriber')
	WITH   
	(
	    SystemSubscriberID int 'SystemSubscriberID', 
		SubscriptionID int 'SubscriptionID', 
		PublicationID int 'PublicationID', 
		account_id int 'account_id'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    

	update impSub
	set impSub.IsMergedToUAD = 'true', impSub.DateMergedToUAD = getdate()
	from ImportSubscriber impSub 
	join @import i on impSub.SystemSubscriberID = i.SystemSubscriberID and 
					  impSub.SubscriptionID = i.SubscriptionID and 
					  impSub.PublicationID = i.PublicationID and
					  impSub.account_id = i.account_id

	update impDim
	set impDim.IsMergedToUAD = 'true', impDim.DateMergedToUAD = getdate()
	from ImportDimension impDim 
	join @import i on impDim.SystemSubscriberID = i.SystemSubscriberID and 
					  impDim.SubscriptionID = i.SubscriptionID and 
					  impDim.PublicationID = i.PublicationID and
					  impDim.account_id = i.account_id

END