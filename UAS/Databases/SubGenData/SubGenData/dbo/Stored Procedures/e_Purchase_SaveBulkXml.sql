create procedure e_Purchase_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[account_id] [int] NOT NULL,
		[billing_address_id] [int] NOT NULL,
		[bundle_id] [int] NULL,
		[invoice_id] [int] NULL,
		[name] [varchar](50) NULL,
		[subscriber_id] [int] NULL,
		IsProcessed bit null,
		ProcessedDate datetime null
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		account_id,billing_address_id,bundle_id,invoice_id,name,subscriber_id,IsProcessed,ProcessedDate
	)  
	
	select account_id,billing_address_id,bundle_id,invoice_id,name,subscriber_id,IsProcessed,ProcessedDate
	from openxml(@docHandle,N'/XML/Purchase')
	with
	(
		account_id int 'account_id',
		billing_address_id int 'billing_address_id',
		bundle_id int 'bundle_id',
		invoice_id int 'invoice_id',
		name varchar(50) 'name',
		subscriber_id int 'subscriber_id',
		IsProcessed bit 'IsProcessed',
		ProcessedDate datetime 'ProcessedDate'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set name = replace(name, '&amp;', '&')

	update @import
	set name = replace(name, '&quot;', '"' )

	update @import
	set name = replace(name, '&lt;', '<')

	update @import
	set name = replace(name, '&gt;', '>')

	update @import
	set name = replace(name, '&apos;', '''')

	insert into Purchase(account_id,billing_address_id,bundle_id,invoice_id,name,subscriber_id,IsProcessed,ProcessedDate)
	select i.account_id,i.billing_address_id,i.bundle_id,i.invoice_id,i.name,i.subscriber_id,i.IsProcessed,i.ProcessedDate
	from @import i

END
go