create procedure e_Payment_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[account_id] [int] NOT NULL,
		[amount] [float] NULL,
		[notes] [varchar](15) NULL,
		[transaction_id] [varchar](15) NULL,
		[type] [varchar](50) NULL,
		[order_id]       INT		  NULL,
		[subscriber_id]  INT          NULL,
		[subscription_id] INT         NULL,
		date_created  datetime        NULL,
		STRecordIdentifier UniqueIdentifier null,
		bundle_id int null
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		account_id,amount,notes,transaction_id,type,order_id,subscriber_id,subscription_id,date_created,STRecordIdentifier,bundle_id
	)  
	
	select account_id,amount,notes,transaction_id,type,order_id,subscriber_id,subscription_id,date_created,STRecordIdentifier,bundle_id
	from openxml(@docHandle,N'/XML/Payment')
	with
	(
		account_id int 'account_id',
		amount float 'amount',
		notes varchar(15) 'notes',
		transaction_id varchar(15) 'transaction_id',
		type varchar(50) 'type',
		order_id int 'order_id',
		subscriber_id int 'subscriber_id',
		subscription_id int 'subscription_id',
		date_created datetime 'date_created',
		STRecordIdentifier uniqueidentifier 'STRecordIdentifier',
		bundle_id int 'bundle_id'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set notes = replace(notes, '&amp;', '&')

	update @import
	set notes = replace(notes, '&quot;', '"' )

	update @import
	set notes = replace(notes, '&lt;', '<')

	update @import
	set notes = replace(notes, '&gt;', '>')

	update @import
	set notes = replace(notes, '&apos;', '''')

	insert into Payment(account_id,amount,notes,transaction_id,type,order_id,subscriber_id,subscription_id,date_created,STRecordIdentifier,bundle_id)
	select i.account_id,i.amount,i.notes,i.transaction_id,i.type,i.order_id,i.subscriber_id,i.subscription_id,i.date_created,i.STRecordIdentifier,i.bundle_id
	from @import i

END
go