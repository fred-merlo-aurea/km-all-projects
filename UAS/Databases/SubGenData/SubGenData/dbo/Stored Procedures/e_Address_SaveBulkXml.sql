create procedure e_Address_SaveBulkXml
@xml xml
as
BEGIN

	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[address_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[first_name] [varchar](50) NULL,
		[last_name] [varchar](50) NULL,
		[address] [varchar](50) NULL,
		[address_line_2] [varchar](50) NULL,
		[company] [varchar](50) NULL,
		[city] [varchar](50) NULL,
		[state] [varchar](60) NULL,
		[subscriber_id] [int] NULL,
		[country] [varchar](50) NULL,
		[country_name] [varchar](50) NULL,
		[country_abbreviation] [varchar](50) NULL,
		[latitude] [decimal](18, 10) NULL,
		[longitude] [decimal](18, 10) NULL,
		[verified] [bit] NULL,
		[zip_code] [varchar](12) NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		 address_id,account_id,first_name,last_name,address,address_line_2,company,city,state,subscriber_id,country,country_name,
		 country_abbreviation,latitude,longitude,verified,zip_code
	)  
	
	select address_id,account_id,first_name,last_name,address,address_line_2,company,city,state,subscriber_id,country,country_name,
		 country_abbreviation,latitude,longitude,verified,zip_code
	from openxml(@docHandle,N'/XML/Address')
	with
	(
		address_id int 'address_id',
		account_id int 'account_id',
		first_name varchar(50) 'first_name',
		last_name varchar(50) 'last_name',
		address varchar(50) 'address',
		address_line_2 varchar(50) 'address_line_2',
		company varchar(50) 'company',
		city varchar(50) 'city',
		state varchar(60) 'state',
		subscriber_id int 'subscriber_id',
		country varchar(50) 'country',
		country_name varchar(50) 'country_name',
		country_abbreviation varchar(50) 'country_abbreviation',
		latitude decimal(18, 10) 'latitude',
		longitude decimal(18, 10) 'longitude',
		verified bit 'verified',
		zip_code varchar(12) 'zip_code'
	)
	
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
	set address = replace(address, '&amp;', '&')

	update @import
	set address = replace(address, '&quot;', '"' )

	update @import
	set address = replace(address, '&lt;', '<')

	update @import
	set address = replace(address, '&gt;', '>')

	update @import
	set address = replace(address, '&apos;', '''')

	update @import
	set address_line_2 = replace(address_line_2, '&amp;', '&')

	update @import
	set address_line_2 = replace(address_line_2, '&quot;', '"' )

	update @import
	set address_line_2 = replace(address_line_2, '&lt;', '<')

	update @import
	set address_line_2 = replace(address_line_2, '&gt;', '>')

	update @import
	set address_line_2 = replace(address_line_2, '&apos;', '''')

	update @import
	set company = replace(company, '&amp;', '&')

	update @import
	set company = replace(company, '&quot;', '"' )

	update @import
	set company = replace(company, '&lt;', '<')

	update @import
	set company = replace(company, '&gt;', '>')

	update @import
	set company = replace(company, '&apos;', '''')

	update @import
	set city = replace(city, '&amp;', '&')

	update @import
	set city = replace(city, '&quot;', '"' )

	update @import
	set city = replace(city, '&lt;', '<')

	update @import
	set city = replace(city, '&gt;', '>')

	update @import
	set city = replace(city, '&apos;', '''')

	update @import
	set state = replace(state, '&amp;', '&')

	update @import
	set state = replace(state, '&quot;', '"' )

	update @import
	set state = replace(state, '&lt;', '<')

	update @import
	set state = replace(state, '&gt;', '>')

	update @import
	set state = replace(state, '&apos;', '''')

	update @import
	set country = replace(country, '&amp;', '&')

	update @import
	set country = replace(country, '&quot;', '"' )

	update @import
	set country = replace(country, '&lt;', '<')

	update @import
	set country = replace(country, '&gt;', '>')

	update @import
	set country = replace(country, '&apos;', '''')

	update @import
	set country_name = replace(country_name, '&amp;', '&')

	update @import
	set country_name = replace(country_name, '&quot;', '"' )

	update @import
	set country_name = replace(country_name, '&lt;', '<')

	update @import
	set country_name = replace(country_name, '&gt;', '>')

	update @import
	set country_name = replace(country_name, '&apos;', '''')

	update @import
	set country_abbreviation = replace(country_abbreviation, '&amp;', '&')

	update @import
	set country_abbreviation = replace(country_abbreviation, '&quot;', '"' )

	update @import
	set country_abbreviation = replace(country_abbreviation, '&lt;', '<')

	update @import
	set country_abbreviation = replace(country_abbreviation, '&gt;', '>')

	update @import
	set country_abbreviation = replace(country_abbreviation, '&apos;', '''')

	update @import
	set zip_code = replace(zip_code, '&amp;', '&')

	update @import
	set zip_code = replace(zip_code, '&quot;', '"' )

	update @import
	set zip_code = replace(zip_code, '&lt;', '<')

	update @import
	set zip_code = replace(zip_code, '&gt;', '>')

	update @import
	set zip_code = replace(zip_code, '&apos;', '''')


	exec sp_xml_removedocument @docHandle

	insert into Address(address_id,account_id,first_name,last_name,address,address_line_2,company,city,state,subscriber_id,country,country_name,
						country_abbreviation,latitude,longitude,verified,zip_code)
	select i.address_id,i.account_id,i.first_name,i.last_name,i.address,i.address_line_2,i.company,i.city,i.state,i.subscriber_id,i.country,i.country_name,
		 i.country_abbreviation,i.latitude,i.longitude,i.verified,i.zip_code
	from @import i
	left join Address a on i.address_id = a.address_id
	where a.address_id is null

	update a
	set a.account_id = i.account_id,
		a.first_name = i.first_name,
		a.last_name = i.last_name,
		a.address = i.address,
		a.address_line_2 = i.address_line_2,
		a.company = i.company,
		a.city = i.city,
		a.state = i.state,
		a.subscriber_id = i.subscriber_id,
		a.country = i.country,
		a.country_name = i.country_name,
		a.country_abbreviation = i.country_abbreviation,
		a.latitude = i.latitude,
		a.longitude = i.longitude,
		a.verified = i.verified,
		a.zip_code = i.zip_code
	from Address a
	join @import i on i.address_id = a.address_id

END
go