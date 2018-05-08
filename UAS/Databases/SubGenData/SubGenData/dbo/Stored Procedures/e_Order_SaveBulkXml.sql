create procedure e_Order_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[order_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[mailing_address_id] [int] NULL,
		[billing_address_id] [int] NULL,
		[subscriber_id] [int] NULL,
		[import_name] [varchar](30) NULL,
		[user_id] [int] NULL,
		[channel_id] [int] NULL,
		[order_date] [datetime] NULL,
		[is_gift] [bit] NULL,
		[sub_total] [float] NULL,
		[tax_total] [float] NULL,
		[grand_total] [float] NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		order_id,account_id,mailing_address_id,billing_address_id,subscriber_id,import_name,user_id,channel_id,order_date,is_gift,sub_total,tax_total,grand_total
	)  
	
	select order_id,account_id,mailing_address_id,billing_address_id,subscriber_id,import_name,user_id,channel_id,order_date,is_gift,sub_total,tax_total,grand_total
	from openxml(@docHandle,N'/XML/Order')
	with
	(
		order_id int 'order_id',
		account_id int 'account_id',
		mailing_address_id int 'mailing_address_id',
		billing_address_id int 'billing_address_id',
		subscriber_id int 'subscriber_id',
		import_name varchar(30) 'import_name',
		user_id int 'user_id',
		channel_id int 'channel_id',
		order_date datetime 'order_date',
		is_gift bit 'is_gift',
		sub_total float 'sub_total',
		tax_total float 'tax_total',
		grand_total float 'grand_total'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set import_name = replace(import_name, '&amp;', '&')

	update @import
	set import_name = replace(import_name, '&quot;', '"' )

	update @import
	set import_name = replace(import_name, '&lt;', '<')

	update @import
	set import_name = replace(import_name, '&gt;', '>')

	update @import
	set import_name = replace(import_name, '&apos;', '''')


	insert into [Order](order_id,account_id,mailing_address_id,billing_address_id,subscriber_id,import_name,user_id,channel_id,order_date,is_gift,sub_total,tax_total,grand_total)
	select i.order_id,i.account_id,i.mailing_address_id,i.billing_address_id,i.subscriber_id,i.import_name,i.user_id,i.channel_id,i.order_date,i.is_gift,
		   i.sub_total,i.tax_total,i.grand_total
	from @import i
	left join [Order] x on i.order_id = x.order_id
	where x.order_id is null

	update x
	set x.account_id = i.account_id,
		x.mailing_address_id = i.mailing_address_id,
		x.billing_address_id = i.billing_address_id,
		x.subscriber_id = i.subscriber_id,
		x.import_name = i.import_name,
		x.user_id = i.user_id,
		x.channel_id = i.channel_id,
		x.order_date = i.order_date,
		x.is_gift = i.is_gift,
		x.sub_total = i.sub_total,
		x.tax_total = i.tax_total,
		x.grand_total = i.grand_total
	from [Order] x
	join @import i on i.order_id = x.order_id

END
go