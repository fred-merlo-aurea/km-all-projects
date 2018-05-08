create procedure e_Bundle_SaveBulkXml
@xml xml
as
BEGIN

	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[bundle_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[name] [varchar](250) NULL,
		[price] [float] NULL,
		[active] [bit] NULL,
		[promo_code] [varchar](25) NULL,
		[description] [varchar](250) NULL,
		[type] [varchar](50) NULL,
		[publication_id] int null,
		[issues] int null
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		bundle_id,account_id,name,price,active,promo_code,description,type,publication_id,issues
	)  
	
	select bundle_id,account_id,name,price,active,promo_code,description,type,publication_id,issues
	from openxml(@docHandle,N'/XML/Bundle')
	with
	(
		bundle_id int 'bundle_id',
		account_id int 'account_id',
		name varchar(250) 'name',
		price float 'price',
		active bit 'active',
		promo_code varchar(25) 'promo_code',
		description varchar(250) 'description',
		type varchar(50) 'type',
		publication_id int 'publication_id',
		issues int 'issues'
	)
	

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

	update @import
	set promo_code = replace(promo_code, '&amp;', '&')

	update @import
	set promo_code = replace(promo_code, '&quot;', '"' )

	update @import
	set promo_code = replace(promo_code, '&lt;', '<')

	update @import
	set promo_code = replace(promo_code, '&gt;', '>')

	update @import
	set promo_code = replace(promo_code, '&apos;', '''')

	update @import
	set description = replace(description, '&amp;', '&')

	update @import
	set description = replace(description, '&quot;', '"' )

	update @import
	set description = replace(description, '&lt;', '<')

	update @import
	set description = replace(description, '&gt;', '>')

	update @import
	set description = replace(description, '&apos;', '''')

	update @import
	set type = replace(type, '&amp;', '&')

	update @import
	set type = replace(type, '&quot;', '"' )

	update @import
	set type = replace(type, '&lt;', '<')

	update @import
	set type = replace(type, '&gt;', '>')

	update @import
	set type = replace(type, '&apos;', '''')

	exec sp_xml_removedocument @docHandle

	insert into Bundle(bundle_id,account_id,name,price,active,promo_code,description,type,publication_id,issues)
	select i.bundle_id,i.account_id,i.name,i.price,i.active,i.promo_code,i.description,i.type,i.publication_id,i.issues
	from @import i
	left join Bundle x on i.bundle_id = x.bundle_id
	where x.bundle_id is null

	update x
	set x.account_id = i.account_id,
		x.name = i.name,
		x.price = i.price,
		x.active = i.active,
		x.promo_code = i.promo_code,
		x.description = i.description,
		x.type = i.type,
		x.publication_id = i.publication_id,
		x.issues = i.issues
	from Bundle x
	join @import i on i.bundle_id = x.bundle_id

END
go