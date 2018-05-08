create procedure job_SubGen_Subscriber_Update
@xml xml
as
BEGIN

	SET NOCOUNT ON

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		subscriber_id int null,
        email varchar(100) null,
        first_name varchar(100) null,
        last_name varchar(100) null,
        renewal_code varchar(50) null,
        source varchar(100) null
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		subscriber_id,email,first_name,last_name,renewal_code,source
	)  
	
	select subscriber_id,email,first_name,last_name,renewal_code,source
	from openxml(@docHandle,N'/ArrayOfSubscriber/Subscriber')
	with
	(
		subscriber_id int 'subscriber_id',
        email varchar(100) 'email',
        first_name varchar(100) 'first_name',
        last_name varchar(100) 'last_name',
        renewal_code varchar(50) 'renewal_code',
        source varchar(100) 'source'
	)
	
	exec sp_xml_removedocument @docHandle

	update ps
	set ps.Email = i.email,
	    ps.FirstName = i.first_name,
		ps.LastName = i.last_name,
		ps.SubscriberSourceCode = i.source,
		ps.DateUpdated = getdate(),
		ps.UpdatedByUserID = 1
	from PubSubscriptions ps
		join @import i on ps.SubGenSubscriberID = i.subscriber_id

	update s
	set s.Email = i.email,
	    s.FName = i.first_name,
		s.LName = i.last_name,
		s.SOURCE = i.source,
		s.DateUpdated = getdate(),
		s.UpdatedByUserID = 1
	from Subscriptions s
		join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID
		join @import i on ps.SubGenSubscriberID = i.subscriber_id

END
go