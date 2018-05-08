create procedure e_User_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[user_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[first_name] [varchar](50) NULL,
		[last_name] [varchar](50) NULL,
		[password] [varchar](25) NULL,
		[password_md5] [varchar](32) NULL,
		[email] [varchar](100) NULL,
		[is_admin] [bit] NULL,
		[KMUserID] [int] NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		[user_id],account_id,first_name,last_name,password,password_md5,email,is_admin,KMUserID
	)  
	
	select [user_id],account_id,first_name,last_name,password,password_md5,email,is_admin,KMUserID
	from openxml(@docHandle,N'/XML/User')
	with
	(
		[user_id] int 'user_id',
		account_id int 'account_id',
		first_name varchar(50) 'first_name',
		last_name varchar(50) 'last_name',
		password varchar(25) 'password',
		password_md5 varchar(32) 'password_md5',
		email varchar(100) 'email',
		is_admin bit 'is_admin',
		KMUserID int 'KMUserID'
	)
	
	exec sp_xml_removedocument @docHandle

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


	insert into [User]([user_id],account_id,first_name,last_name,password,password_md5,email,is_admin,KMUserID)
	select i.[user_id],i.account_id,i.first_name,i.last_name,i.password,i.password_md5,i.email,i.is_admin,i.KMUserID
	from @import i
	left join [User] x on i.[user_id] = x.[user_id]
	where x.[user_id] is null

	update x
	set x.account_id = i.account_id,
		x.first_name = i.first_name,
		x.last_name = i.last_name,
		x.password = i.password,
		x.password_md5 = i.password_md5,
		x.email = i.email,
		x.is_admin = i.is_admin,
		x.KMUserID = i.KMUserID
	from [User] x
	join @import i on i.[user_id] = x.[user_id]

END
go