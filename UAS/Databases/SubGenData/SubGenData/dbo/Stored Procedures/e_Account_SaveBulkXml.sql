create procedure e_Account_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		account_id int,
		company_name varchar(50),
		email varchar(100),
		website varchar(100),
		active bit,
		api_active bit,
		api_key varchar(100),
		api_login varchar(50),
		[plan] varchar(50),
		audit_type varchar(50),
		created datetime,
		master_checkout varchar(255),
		KMClientId int
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		 account_id,company_name,email,website,active,api_active,api_key,api_login,[plan],audit_type,created,master_checkout,KMClientId
	)  
	
	select account_id,company_name,email,website,active,api_active,api_key,api_login,[plan],audit_type,created,master_checkout,KMClientId
	from openxml(@docHandle,N'/XML/Account')
	with
	(
		account_id int 'account_id',
		company_name varchar(50) 'company_name',
		email varchar(100) 'email',
		website varchar(100) 'website',
		active bit 'active',
		api_active bit 'api_active',
		api_key varchar(100) 'api_key',
		api_login varchar(50) 'api_login',
		[plan] varchar(50) 'plan',
		audit_type varchar(50) 'audit_type',
		created datetime 'created',
		master_checkout varchar(255) 'master_checkout',
		KMClientId int 'KMClientId'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set company_name = replace(company_name, '&amp;', '&')

	update @import
	set company_name = replace(company_name, '&quot;', '"' )

	update @import
	set company_name = replace(company_name, '&lt;', '<')

	update @import
	set company_name = replace(company_name, '&gt;', '>')

	update @import
	set company_name = replace(company_name, '&apos;', '''')





	insert into Account(account_id,company_name,email,website,active,api_active,api_key,api_login,[plan],audit_type,created,master_checkout,KMClientId)
	select i.account_id,i.company_name,i.email,i.website,i.active,i.api_active,i.api_key,i.api_login,i.[plan],i.audit_type,i.created,i.master_checkout,i.KMClientId
	from @import i
	left join Account a on i.account_id = a.account_id
	where a.account_id is null

	update a
	set a.company_name = i.company_name,
		a.email = i.email,
		a.website = i.website,
		a.active = i.active,
		a.api_active = i.api_active,
		a.api_key = i.api_key,
		a.api_login = i.api_login,
		a.[plan] = i.[plan],
		a.audit_type = i.audit_type,
		a.master_checkout = i.master_checkout,
		a.KMClientId = case when i.KMClientId = 0 then a.KMClientId else i.KMClientId end
	from Account a
	join @import i on i.account_id = a.account_id

END
go