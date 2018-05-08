create procedure e_Subscriber_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[subscriber_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[renewal_code] [varchar](5) NULL,
		[email] [varchar](100) NULL,
		[password] [varchar](25) NULL,
		[password_md5] [varchar](32) NULL,
		[first_name] [varchar](25) NULL,
		[last_name] [varchar](25) NULL,
		[source] [varchar](100) NULL,
		[create_date] [datetime] NULL,
		[delete_date] [datetime] NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		subscriber_id,account_id,renewal_code,email,password,password_md5,first_name,last_name,source,create_date,delete_date
	)  
	
	select subscriber_id,account_id,renewal_code,email,password,password_md5,first_name,last_name,source,create_date,delete_date
	from openxml(@docHandle,N'/XML/Subscriber')
	with
	(
		subscriber_id int 'subscriber_id',
		account_id int 'account_id',
		renewal_code varchar(5) 'renewal_code',
		email varchar(100) 'email',
		password varchar(25) 'password',
		password_md5 varchar(32) 'password_md5',
		first_name varchar(25) 'first_name',
		last_name varchar(25) 'last_name',
		source varchar(100) 'source',
		create_date datetime 'create_date',
		delete_date datetime 'delete_date'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set renewal_code = replace(renewal_code, '&amp;', '&')

	update @import
	set renewal_code = replace(renewal_code, '&quot;', '"' )

	update @import
	set renewal_code = replace(renewal_code, '&lt;', '<')

	update @import
	set renewal_code = replace(renewal_code, '&gt;', '>')

	update @import
	set renewal_code = replace(renewal_code, '&apos;', '''')

	update @import
	set email = replace(email, '&amp;', '&')

	update @import
	set email = replace(email, '&quot;', '"' )

	update @import
	set email = replace(email, '&lt;', '<')

	update @import
	set email = replace(email, '&gt;', '>')

	update @import
	set email = replace(email, '&apos;', '''')

	update @import
	set password = replace(password, '&amp;', '&')

	update @import
	set password = replace(password, '&quot;', '"' )

	update @import
	set password = replace(password, '&lt;', '<')

	update @import
	set password = replace(password, '&gt;', '>')

	update @import
	set password = replace(password, '&apos;', '''')

	update @import
	set first_name = replace(first_name, '&amp;', '&')

	update @import
	set first_name = replace(first_name, '&quot;', '"' )

	update @import
	set first_name = replace(first_name, '&lt;', '<')

	update @import
	set first_name = replace(first_name, '&gt;', '>')

	update @import
	set first_name = replace(first_name, '&apos;', '''')

	update @import
	set last_name = replace(last_name, '&amp;', '&')

	update @import
	set last_name = replace(last_name, '&quot;', '"' )

	update @import
	set last_name = replace(last_name, '&lt;', '<')

	update @import
	set last_name = replace(last_name, '&gt;', '>')

	update @import
	set last_name = replace(last_name, '&apos;', '''')

	update @import
	set source = replace(source, '&amp;', '&')

	update @import
	set source = replace(source, '&quot;', '"' )

	update @import
	set source = replace(source, '&lt;', '<')

	update @import
	set source = replace(source, '&gt;', '>')

	update @import
	set source = replace(source, '&apos;', '''')


	insert into Subscriber(subscriber_id,account_id,renewal_code,email,password,password_md5,first_name,last_name,source,create_date,delete_date)
	select i.subscriber_id,i.account_id,i.renewal_code,i.email,i.password,i.password_md5,i.first_name,i.last_name,i.source,i.create_date,i.delete_date
	from @import i
	left join Subscriber x on i.subscriber_id = x.subscriber_id
	where x.subscriber_id is null

	update x
	set x.account_id = i.account_id,
		x.renewal_code = i.renewal_code,
		x.email = i.email,
		x.password = i.password,
		x.password_md5 = i.password_md5,
		x.first_name = i.first_name,
		x.last_name = i.last_name,
		x.source = i.source,
		x.create_date = i.create_date,
		x.delete_date = i.delete_date
	from Subscriber x
	join @import i on i.subscriber_id = x.subscriber_id

END
GO